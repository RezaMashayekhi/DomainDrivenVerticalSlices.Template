---
name: dependency-security-review
description: Review and update NuGet dependencies, resolved licenses, lock files, and vulnerability findings in both template trees.
---

# Dependency review

Versions live in the two `Directory.Packages.props` files and the Aspire SDK attribute
in both AppHost projects. Read `docs/DEPENDENCIES.md` for compatibility holds and policy.
Do not duplicate package versions in skill prose.

```powershell
python scripts/dependency-audit.py --updates --output artifacts/dependency-updates.json
```

The report shows latest stable and same-major candidates; it does not edit files or
prove API compatibility. Never downgrade a prerelease analyzer just to choose a stable
version. Review major upgrades and transitive changes before applying them.

Update both trees where the dependency exists. Restore both solutions with
`--force-evaluate -p:RestoreLockedMode=false`, then commit the refreshed lock files.
AppHost locked restore is disabled because Aspire injects platform-specific packages.

```powershell
python scripts/dependency-audit.py --output artifacts/dependency-licenses.json --fail-on-review
dotnet list DomainDrivenVerticalSlices.Template.slnx package --vulnerable --include-transitive --no-restore
dotnet list templates/ddvs-modular/DomainDrivenVerticalSlices.Template.slnx package --vulnerable --include-transitive --no-restore
```

The license audit reads resolved direct/transitive packages, not just direct references.
File-based or legacy licenses require an exact-version review in `scripts/license-reviews.json`,
including the evidence hash and source. Never approve a fee-bearing binary agreement on the
basis of an upstream MIT source license. New or changed unknown licenses block the gate.
Dependencies retain their own licenses and notice obligations.

Run the full `scripts/validate-template.ps1` matrix after updating dependencies. Record
actual results and retained versions with reasons. If a feed is unavailable, report the
check as incomplete; do not turn network failures into clean audit results.

For React dependency changes, use `npm ci`, `npm audit`, and the generated React checks;
NuGet license evidence does not certify npm dependencies.
