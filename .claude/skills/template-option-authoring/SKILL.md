---
name: template-option-authoring
description: Use when adding, renaming, or changing a dotnet-new template option/symbol (like --ApiType, --UiType, --UseAspire), its choices, or its defaults in any template.json. Walks through symbol design, lockstep config mirroring, conditional content, smoke-matrix extension, and doc updates.
---

# Template Option Authoring

Adding or changing a `template.json` option is this repo's most error-prone recurring change:
it spans **three configs**, conditional source, the validation matrix, and four doc surfaces.
Follow every step; a missed one ships silently broken output.

## 0. Breaking-change check first

- Renaming/removing an option or **changing a default** changes what users get → breaking
  (`docs/MAINTAINER_GUIDE.md` §12): requires explicit permission and migration notes.
- Adding a new option with a default that preserves today's output is non-breaking.

## 1. Design the symbol

Follow the existing pattern (`.template.config/template.json`):

```jsonc
"MyOption": {
    "type": "parameter",
    "datatype": "choice",          // or "bool" (see UseAspire in the modular config)
    "choices": [
        { "choice": "ValueA", "description": "..." },
        { "choice": "ValueB", "description": "..." }
    ],
    "defaultValue": "ValueA",
    "description": "..."
},
"INCLUDE_VALUE_A": { "type": "computed", "value": "(MyOption == \"ValueA\")" },
"INCLUDE_VALUE_B": { "type": "computed", "value": "(MyOption == \"ValueB\")" }
```

Computed `INCLUDE_*` / `USE_*` flags are what conditions and modifiers consume — never test
the raw parameter in content.

## 2. Mirror into every affected config

- Clean Architecture option → edit **both** `.template.config/template.json` (ddvs) and
  `templates/ddvs-clean/.template.config/template.json` (ddvs-clean). They must stay
  lockstep-identical (symbols, choices, modifiers, excludes) — only the `sources.source`
  path differs (`./` vs `../../`).
- Modular option → only `templates/ddvs-modular/.template.config/template.json`.
- Do not add Clean options to the modular config or vice versa (`--ApiType`/`--UiType` are
  Clean-only; `--UseAspire` is modular-only). Update `docs/MAINTAINER_GUIDE.md` §2 if the option set changes.

## 3. Wire conditional content

- **Exclude whole files/folders** with a `sources.modifiers` entry:

  ```jsonc
  { "condition": "(INCLUDE_VALUE_B)", "exclude": [ "src/.../SomeValueAOnlyFolder/**" ] }
  ```

  Mutually exclusive content needs a modifier per branch (see `INCLUDE_CONTROLLERS` /
  `INCLUDE_MINIMAL_API`).
- **C# lines**: `#if INCLUDE_VALUE_A` ... `#endif`. Do **not** leave blank lines directly
  around `#if`/`#endif` — StyleCop SA1507 fails the generated build. Gate any needed
  `DefineConstants` in the `.csproj` with `<!--#if (…) -->` (see the modular Api csproj).
- **XML/slnx/json**: `<!--#if (USE_ASPIRE)-->` ... `<!--#endif-->` (see the modular `.slnx`).

## 4. Extend validation

- Add the new option's combos to the smoke matrix **and** the §5b comprehensive matrix in
  `dotnet-template-validation` (both variants for Clean options), plus generated-output
  assertions for what the option includes/excludes (mirror the `--UseAspire false` and
  `--ApiType` assertions).
- Run the full pack → install → generate → build/test → assert flow for **every value** of
  the new option, on every variant that has it. `ddvs` and `ddvs-clean` must produce
  identical output for identical options.

## 5. Update the documentation surfaces (same change)

- README options/parameters section + example commands.
- `docs/MAINTAINER_GUIDE.md` §2 options table.
- `.nuspec` `<description>`/`<releaseNotes>` if user-facing.
- `CHANGELOG.md` (`Added` for a new option; `Changed` + migration notes for a default change).
- The `documentation-consistency` skill's checks then apply — run it before finishing.
