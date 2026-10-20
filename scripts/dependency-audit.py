"""Inspect NuGet versions and licenses using official package metadata (Python stdlib only)."""
import argparse
import concurrent.futures
import hashlib
import io
import json
import os
from pathlib import Path
import urllib.request
import xml.etree.ElementTree as ET
import zipfile

ROOT = Path(__file__).resolve().parents[1]
FEED = "https://api.nuget.org/v3-flatcontainer"
PERMISSIVE = {"MIT", "Apache-2.0", "BSD-2-Clause", "BSD-3-Clause", "ISC", "0BSD", "Unlicense", "CC0-1.0"}
REVIEWS = json.loads((ROOT / 'scripts/license-reviews.json').read_text(encoding='utf-8'))


def get(url):
    with urllib.request.urlopen(url, timeout=60) as response:
        return response.read()


def metadata(package, version):
    url = f"{FEED}/{package.lower()}/{version.lower()}/{package.lower()}.nuspec"
    cache = Path(os.environ.get('NUGET_PACKAGES', str(Path.home() / '.nuget/packages')))
    cached_nuspec = cache / package.lower() / version.lower() / f"{package.lower()}.nuspec"
    raw = cached_nuspec.read_bytes() if cached_nuspec.exists() else get(url)
    root = ET.fromstring(raw)
    values = {element.tag.split('}')[-1]: element for element in root.iter()}
    license_node = values.get('license')
    expression = license_node.text.strip() if license_node is not None and license_node.text else ""
    kind = license_node.get('type', '') if license_node is not None else ''
    result = {"package": package, "version": version, "license": expression,
            "licenseType": kind, "metadataUrl": url,
            "licenseUrl": values['licenseUrl'].text if 'licenseUrl' in values else '',
            "review": "permissive" if kind == 'expression' and expression in PERMISSIVE else "review-required"}
    review = REVIEWS.get(f'{package.lower()}/{version.lower()}')
    if review and result['review'] == 'review-required':
        if kind == 'file':
            license_file = cache / package.lower() / version.lower() / expression
            if license_file.exists():
                evidence = license_file.read_bytes()
            else:
                with zipfile.ZipFile(io.BytesIO(get(f'{FEED}/{package.lower()}/{version.lower()}/{package.lower()}.{version.lower()}.nupkg'))) as archive:
                    evidence = archive.read(expression)
        else:
            evidence = raw
        digest = hashlib.sha256(evidence).hexdigest()
        result['evidenceSha256'] = digest
        if digest == review['sha256'].lower():
            result.update(review='permissive-reviewed', reviewedLicense=review['license'], rationale=review['rationale'], evidenceUrl=review['source'])
    return result


def version_key(version):
    return tuple(int(part) for part in version.split('.'))


def updates(entry):
    package, current = entry
    versions = json.loads(get(f"{FEED}/{package.lower()}/index.json"))['versions']
    stable = [v for v in versions if '-' not in v]
    latest = max(stable, key=version_key)
    same_major = [v for v in stable if v.split('.')[0] == current.split('.')[0]]
    candidate = max(same_major, key=version_key) if same_major else current
    if '-' in current and version_key(current.split('-')[0]) > version_key(candidate):
        candidate = current  # Never silently downgrade an existing prerelease analyzer.
    result = metadata(package, candidate)
    result.update(current=current, latest=latest, candidate=candidate)
    return result


def collect_resolved():
    packages = set()
    assets_files = []
    for source in (ROOT / 'src', ROOT / 'test', ROOT / 'templates/ddvs-modular/src', ROOT / 'templates/ddvs-modular/tests'):
        for project in source.glob('*/*.csproj'):
            assets = project.parent / 'obj/project.assets.json'
            if not assets.exists():
                raise RuntimeError(f"Restore first: missing {assets}")
            assets_files.append(str(assets.relative_to(ROOT)))
            data = json.loads(assets.read_text(encoding='utf-8-sig'))
            for name, library in data['libraries'].items():
                if library['type'] == 'package':
                    packages.add(tuple(name.rsplit('/', 1)))
    # Aspire's SDK is selected in the project Sdk attribute and is therefore not
    # represented by project.assets.json. Include it explicitly in the license graph.
    for project in ROOT.glob('**/*AppHost.csproj'):
        if 'artifacts' not in project.parts:
            sdk = ET.parse(project).getroot().get('Sdk', '')
            if '/' in sdk:
                packages.add(tuple(sdk.split('/')))
    if not packages:
        raise RuntimeError('No resolved NuGet packages found')
    return sorted(packages), assets_files


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--updates', action='store_true')
    parser.add_argument('--output', required=True)
    parser.add_argument('--fail-on-review', action='store_true')
    args = parser.parse_args()
    if args.updates:
        entries = set()
        for file in (ROOT / 'Directory.Packages.props', ROOT / 'templates/ddvs-modular/Directory.Packages.props'):
            entries.update((n.attrib['Include'], n.attrib['Version']) for n in ET.parse(file).iter('PackageVersion'))
        for file in ROOT.glob('**/*AppHost.csproj'):
            if 'artifacts' not in file.parts:
                sdk = ET.parse(file).getroot().get('Sdk', '')
                if '/' in sdk:
                    entries.add(tuple(sdk.split('/')))
        worker = updates
        assets_files = []
    else:
        entries, assets_files = collect_resolved()
        worker = lambda entry: metadata(*entry)
    with concurrent.futures.ThreadPoolExecutor(max_workers=8) as executor:
        rows = list(executor.map(worker, sorted(entries)))
    output = Path(args.output)
    output.parent.mkdir(parents=True, exist_ok=True)
    output.write_text(json.dumps({'source': FEED, 'assetsFiles': assets_files, 'packages': rows}, indent=2) + '\n', encoding='utf-8')
    unresolved = [r for r in rows if not r['review'].startswith('permissive')]
    print(f"Inspected {len(rows)} packages; {len(unresolved)} require license review. Report: {output}")
    for row in unresolved:
        print(f"REVIEW {row['package']} {row['version']}: {row['licenseType']} {row['license']}")
    if args.fail_on_review and unresolved:
        raise SystemExit(1)


if __name__ == '__main__':
    main()
