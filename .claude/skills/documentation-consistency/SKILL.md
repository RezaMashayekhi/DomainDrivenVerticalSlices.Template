---
name: documentation-consistency
description: Check that maintainer guidance, release notes, template options, and shipped documentation match the code and packaging boundaries.
---

# Documentation consistency

`docs/MAINTAINER_GUIDE.md` is authoritative. Root AI entry points link to it;
`docs/ROADMAP.md` is the active plan. Keep current facts out of historical release entries.

Check the nuspec version against README install instructions and React package metadata.
Check all three template configs and their actual source trees before documenting options.
Use current props/global.json for versions and prerequisites instead of hardcoded skill prose.

Shipped README files describe generated applications. Maintainer rules, scripts, dependency
reports, changelogs and historical plans must be excluded from the package and generation.
Update the nuspec and both Clean template excludes when adding maintainer files; modular
already excludes its docs and .github directories. Run package validation to verify it.

When behavior changes, record migration notes and update the roadmap status and acceptance
criteria. Commands should reference the maintained validation script rather than copied matrices.
Check Markdown links relative to the file containing them.
