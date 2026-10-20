---
name: release-notes-and-commit
description: Record meaningful repository changes, validation evidence, migration notes and a proposed conventional commit without publishing.
---

# Release notes and commits

Record changes under `CHANGELOG.md` Unreleased. For changed generated output or packages,
bump the nuspec version and release notes and align the README install version and React
package version. Do not invent a release date or claim the candidate has been published.

Report changed behavior, affected variants, migration steps, actual validation results,
and any remaining risks. Evaluate both source trees and explain legitimate single-tree
changes. A short response is sufficient when the change is small; omit empty categories.

Propose `<type>(<scope>): <summary>`. Stage or commit only if the user requests it; never
push, tag, publish or open a PR without authorization. Publishing uses the validated
artifact through `.github/workflows/package.yml`; it must not repack after validation.
