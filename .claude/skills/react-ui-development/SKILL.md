---
name: react-ui-development
description: Use when changing anything under src/DomainDrivenVerticalSlices.Template.UI.React (components, deps, Vite config, tests) or the Aspire wiring that hosts it. Covers the npm workflow, Vitest+MSW testing, the AddViteApp/dynamic-port contract, INCLUDE_REACT gating, and required validation.
---

# React UI Development

The React project is `src/DomainDrivenVerticalSlices.Template.UI.React/` (React 19, Vite 6,
Tailwind CSS v4, Vitest + MSW). It ships **only** when users pass `--UiType React` to
`ddvs`/`ddvs-clean`; the modular template has no UI.

## Daily loop

```bash
cd src/DomainDrivenVerticalSlices.Template.UI.React
npm ci            # NOT npm install — a lockfile exists and must stay authoritative
npm run dev       # local dev server
npm run build     # production build (Vite → dist/)
npm test          # Vitest (+ MSW mocks for API calls)
npm run lint      # if configured in package.json
```

- If you intentionally change dependencies: edit `package.json`, run `npm install` **once**
  to regenerate `package-lock.json`, commit both, then verify `npm ci` passes clean.
- The project `version` field in `package.json`/`package-lock.json` tracks the **template
  package version** (`.nuspec <version>`) — bump it with releases.
- `npm audit` findings: fix by pinning/upgrading in `package.json` + lockfile and record
  CVE/GHSA ids in `CHANGELOG.md`/release notes (see `dependency-security-review`).

## The Aspire ↔ Vite contract (do not break)

- `*.AppHost/Program.cs` registers the UI with `AddViteApp("reactui", ...)` and
  `.WithReference(webApi)` — all inside `#if INCLUDE_REACT`.
- `vite.config.js` reads the WebApi address from the Aspire-injected environment variable
  `services__webapi__https__0`. **Never hardcode ports** on either side.
- StyleCop SA1507: no blank lines directly around `#if INCLUDE_REACT`/`#endif` in AppHost
  `Program.cs` — the `--UiType None` build fails otherwise.

## Shipping rules

- The UI folder is excluded from generation when `--UiType None` (template configs'
  `INCLUDE_API_ONLY` modifier). Adding React-only files under other projects? Gate them.
- These files **must ship** with the UI: `package-lock.json` (generated apps run `npm ci`)
  and the project `.gitignore` (ignores `node_modules/`, `coverage/`, `dist/`).
  Build outputs (`dist/`, `coverage/`) must **never** ship — they are nuspec-excluded.

## Done means validated

A UI change is finished only when, per `dotnet-template-validation`:

1. Both trees still build 0-warning (`--UiType None` compiles the `#if` branches out).
2. A **generated** `--UiType React` project builds (`dotnet build -c Release`) and its UI
   passes `npm ci && npm run build && npm test`.
3. Generated-output assertions pass (no template-name leak, no dev files, lockfile present).
