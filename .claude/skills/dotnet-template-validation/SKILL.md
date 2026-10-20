---
name: dotnet-template-validation
description: Validate source, NuGet packaging, and all generated template options after template, dependency, or release changes. Uses the same executable checks as CI.
---

# Template validation

Read `docs/MAINTAINER_GUIDE.md` for template boundaries. Run from the repository root:

```powershell
pwsh -File scripts/validate-template.ps1
```

Requires .NET 10 (see global.json), PowerShell 7, Python 3.12+, Node.js, and NuGet CLI
(`nuget.exe`, or `nuget` with Mono, as appropriate for the host).

The script builds/tests both source trees, audits resolved NuGet licenses, packs with
`-NoDefaultExcludes`, and generates all ten configurations from that exact package.
It checks package exclusions, generated file/name leaks, option gating, normalized
alias parity, and builds/tests every generated solution and all four React outputs.
Assertions run before restore so newly generated lock files are not mistaken for leaks.

Scratch projects and an isolated template hive are created under the OS temp directory.
No global template uninstall is needed. Retain the printed scratch path when reporting
failures; cleanup is manual after diagnostics are no longer needed.

Only successful validation copies the package into `artifacts/validation/` and writes
its SHA-256, source commit, and matrix to `validation.json`. Publishing consumes that
artifact without repacking. Use a fresh output directory for each standalone release run.

`-SourceOnly` is useful while iterating, but is not a release or generated-output gate.
A user-requested full validation already authorizes the full matrix; do not ask again.
For other changes, agree on scope using the available conversation interface if needed.
Never report success when a required command was skipped or failed.
