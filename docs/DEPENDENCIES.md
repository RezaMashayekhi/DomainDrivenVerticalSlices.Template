# Dependency policy

The template package and repository are MIT-licensed. Dependencies retain their own
licenses; Apache-2.0, BSD, ISC, public-domain, and similarly permissive terms are acceptable
when they allow use and redistribution in this repository and in generated projects. Do not
copy the repository's MIT label onto third-party code. Preserve required notices and source
attributions. Reject terms that require fees, restrict ordinary redistribution, or impose
obligations incompatible with this repository's use.

`scripts/dependency-audit.py` inspects resolved direct and transitive NuGet package metadata
for the current host. Aspire's SDK package is added explicitly because it is selected through
the AppHost project `Sdk` attribute and does not appear in `project.assets.json`. The audit
uses cached nuspec/license files when available and the official NuGet flat-container feed
otherwise. A license that needs interpretation must have exact-version evidence and a hash in
`scripts/license-reviews.json`; an unknown license fails validation. NuGet's licensing
metadata is not a substitute for retaining notices when a package requires them.

The resulting package graph is host-specific. Aspire injects operating-system-specific
packages, so CI and local runs provide evidence for their respective hosts separately;
neither report claims to cover every supported operating system. Generated projects restore
their own graph when users choose their target platform.

Versions are centrally managed in the root and modular `Directory.Packages.props` files.
Lock files are committed for both template trees. Aspire AppHost projects disable locked
restore because Aspire adds host-specific packages. Keep the Aspire SDK version aligned across
both trees and review major upgrades for compatibility before applying them.

Use the audit and full validation script:

```powershell
python scripts/dependency-audit.py --updates --output artifacts/dependency-updates.json
python scripts/dependency-audit.py --output artifacts/dependency-licenses.json --fail-on-review
pwsh -File scripts/validate-template.ps1
```

The shared validator also runs `dotnet list package --vulnerable --include-transitive` for
both source solutions and `npm audit --offline=false --fetch-retries=0 --fetch-timeout=30000`
for the React graph. Any advisory or feed/command error fails the gate. Do not characterize an
incomplete audit as clean.

React/npm is a separate dependency graph. The 10.8.0 UI dependency lock was
refreshed on 2026-09-27; the 2026-10-18 audit found GHSA-68fv-2mgg-jv7q in transitive
`source-map-js` 1.2.1. The lock now pins 1.2.2, the fixed BSD-3-Clause release, without changing
the package manifest ranges; the online audit on 2026-10-18 reported zero advisories. The previous 12
advisories (7 moderate, 5 high) were resolved. React Router was upgraded from 6.30.6 to 7.18.4
because the fixed release is in the 7.x line; the template uses its basic declarative routing
APIs. Other package majors remain on their current lines where latest releases would require
toolchain or API migrations (Vite 6, Vitest 4, ESLint 9, jsdom 26, Testing Library jest-dom 6,
globals 16, and web-vitals 5). Revisit these separately with compatibility tests before moving
to new majors.

The resolved npm graph includes permissive licenses such as MIT, ISC, Apache-2.0, BSD,
BlueOak-1.0.0, and 0BSD. It also includes licenses with specific notice or source obligations:
Lightning CSS 1.32.0 is MPL-2.0, caniuse-lite 1.0.30001812 is CC-BY-4.0, and argparse 2.0.1 is
Python-2.0. No fee-bearing or proprietary terms were identified. Keep each package's license
text and notices, provide attribution where required, and follow MPL source-disclosure terms
when distributing modified covered files.

Current compatibility holds: Aspire 13.4.6 is retained. The latest Aspire line verified on
2026-09-27 is 13.5.4; a temporary restore of that graph included Json.More.Net 3.0.1,
JsonPatch.Net 5.0.2, and JsonPointer.Net 7.0.1 with `OSMFEULA.txt` agreements that can require
maintenance fees for qualifying revenue-generating use. MessagePack 2.x and Microsoft.OpenApi
2.x are retained to avoid unreviewed major API changes; the prerelease StyleCop analyzer is
retained rather than downgraded to the previous stable line.
