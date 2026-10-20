# Release Process

How to validate, package, and (when approved) publish
`RM.DomainDrivenVerticalSlices.Template`. This process is validation-driven and conservative.

> **Do not publish to NuGet, create tags, or create GitHub releases without explicit
> maintainer approval.** Every release must include release notes. Every meaningful change
> must include a proposed conventional commit. See `docs/MAINTAINER_GUIDE.md` §12.

## 1. Versioning policy

- The package version is the single `<version>` in
  `DomainDrivenVerticalSlices.Template.nuspec` and is the source of truth for the package being
  prepared. Check [NuGet.org](https://www.nuget.org/packages/RM.DomainDrivenVerticalSlices.Template)
  for publication status and the latest published version; a repository version does not prove
  publication.
- Semantic-ish versioning for a template package:
  - **Major** — breaking change to generated structure, template short names/identities,
    `sourceName`, or removal/renaming of options/defaults. Requires migration notes.
  - **Minor** — new template variant or option, new opt-in feature, dependency upgrades that
    change generated behavior.
  - **Patch** — fixes and security/dependency bumps with no user-facing option changes.
- Bump `<version>` and prepend a `Version X.Y.Z:` block to `<releaseNotes>` in the same edit.

## 2. Pre-release checklist

- [ ] Working tree clean; changes on a branch (not `main`).
- [ ] `.nuspec` `<version>` bumped; `<releaseNotes>` updated.
- [ ] `CHANGELOG.md` has a dated section for the new version.
- [ ] README / docs / all three `template.json` configs consistent
      (`documentation-consistency` skill).
- [ ] `Directory.Packages.props` (root + modular) and lock files updated intentionally.
- [ ] Breaking changes have migration notes.

## 3. Validation commands

```bash
git diff --check
dotnet --info                                  # SDK 10.0.102+ (global.json)
pwsh -File scripts/validate-template.ps1
```

The script is the canonical gate used locally and in CI. It restores, builds, and tests both
source trees; audits resolved direct/transitive NuGet licenses and vulnerabilities; packs with
`nuget.exe`/`nuget`; installs into an isolated template hive; and generates, builds, tests, and
asserts all ten configurations, including React. It leaves diagnostics in a temporary directory.
All builds treat warnings as errors.

## 4. Smoke-test matrix

The full ten-configuration matrix is the standard release gate. For targeted local iteration,
`pwsh -File scripts/validate-template.ps1 -SourceOnly` omits packing and generated-output checks;
it does not qualify as release validation.

## 5. NuGet package verification

- Pack with `nuget.exe pack DomainDrivenVerticalSlices.Template.nuspec -OutputDirectory artifacts -NoDefaultExcludes`
  (never `dotnet pack`).
- Confirm the produced `artifacts/RM.DomainDrivenVerticalSlices.Template.<version>.nupkg`:
  - contains `.editorconfig` and other dotfiles (proves `-NoDefaultExcludes` worked),
  - contains the intentionally shipped docs: `content/README.md`, `content/docs/screenshots/**`,
    the React project `.gitignore` and `package-lock.json`, and the per-project READMEs
    (including the modular `Modules/<Module>/README.md`),
  - does **not** contain `.github/workflows/**`, `.github/instructions/**`, `artifacts/**`,
    `nupkgs/**`, `*.nupkg`, `nuget.exe`, `packages.lock.json`, `.template.config` dev leftovers
    beyond the three template configs, the modular tree's `.github/**` or `docs/**` (Copilot
    instructions + architecture ADRs are repo-only), **nor any repo-dev/AI guidance**:
    `.claude/**`, `CLAUDE.md`, `AGENTS.md`, `CHANGELOG.md`, `docs/ROADMAP.md`, `docs/RELEASE_PROCESS.md`,
    `*.code-workspace`,
  - lists all three templates: `dotnet new list ddvs|ddvs-clean|ddvs-modular` against a custom hive.
- Quick content check (the `.nupkg` is a zip):

  ```powershell
  Add-Type -AssemblyName System.IO.Compression.FileSystem
  $z = [System.IO.Compression.ZipFile]::OpenRead((Resolve-Path 'artifacts/RM.DomainDrivenVerticalSlices.Template.<version>.nupkg'))
  $z.Entries.FullName | Where-Object { $_ -match 'claude|CLAUDE|CHANGELOG|ROADMAP|RELEASE_PROCESS|code-workspace|packages\.lock\.json' }   # must return nothing
  $z.Dispose()
  ```

## 6. Release notes checklist

- [ ] `.nuspec <releaseNotes>` has a new `Version X.Y.Z:` block describing real changes.
- [ ] `CHANGELOG.md` updated (Keep-a-Changelog categories and date).
- [ ] README "What's New"/highlights updated if user-facing (keep concise; don't skip versions).
- [ ] README pinned install version (`dotnet new install RM.DomainDrivenVerticalSlices.Template@X.Y.Z`)
      and the React `package.json`/`package-lock.json` project version updated to the new version.
- [ ] Final release notes produced in the `release-notes-and-commit` format, including a
      `Validation` section with real command results.

## 7. Commit and tag conventions

- Conventional commits: `<type>(<scope>): <summary>` (`feat|fix|docs|test|refactor|build|ci|
  chore|security`). Breaking: `type!:` + `BREAKING CHANGE:` footer with migration notes.
- Tags (when approved): `vX.Y.Z` matching `.nuspec <version>`.
- **Do not create tags or push without explicit approval.**

## 8. Publish workflow warning

Publishing is a **manual, approval-gated** GitHub Action: `.github/workflows/package.yml`
(`workflow_dispatch`, requires typing `publish`). It downloads the artifact produced by the
shared validation workflow, checks the commit and SHA-256 evidence, then pushes that exact
package to NuGet.org using `secrets.NUGET_API_KEY`.

The publish job runs only after the shared validation job succeeds. Do not run it without
explicit maintainer approval.

Do not run this workflow, or otherwise `nuget push`, without explicit maintainer approval.

## 9. Post-release verification

- Confirm the version appears on nuget.org and the README renders.
- In a clean environment: `dotnet new install RM.DomainDrivenVerticalSlices.Template@<version>`
  then generate + build one project per variant.
- Verify the CI `Build` and `CodeQL` badges are green for the released commit.

## 10. Rollback / unpublish considerations

- NuGet does **not** support true deletion; you can only *unlist* a version
  (`nuget delete <id> <version> -Source ...` unlists, it does not remove). Unlisting hides it
  from search but existing pins still resolve.
- Preferred remediation for a bad release: **publish a fixed higher patch version** rather than
  relying on unlisting.
- If a release must be pulled, unlist it, publish the fix, and document both in `CHANGELOG.md`
  and `.nuspec` release notes. Unlisting/deleting requires maintainer approval.
