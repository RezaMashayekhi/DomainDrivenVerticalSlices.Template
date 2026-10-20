# Maintainer guide

Guidance for Claude Code (and other AI agents) working in this repository. Read this
before making changes. It is deliberately practical: exact commands and hard rules over
generic advice.

> This repository is a **NuGet-distributed .NET template package**, not a normal
> application. Almost everything under `src/`, `test/`, and `templates/` is *template
> source* that ships to users. A change here becomes code in every project people
> generate. Treat it accordingly.

---

## 1. Project purpose

`RM.DomainDrivenVerticalSlices.Template` is an ASP.NET Core template package implementing
Domain-Driven Design (DDD) with Vertical Slice Architecture on **.NET 10**. It ships
multiple `dotnet new` template variants from a single NuGet package
(`DomainDrivenVerticalSlices.Template.nuspec`). Package version is defined in the `.nuspec`; do not duplicate it in instructions.

## 2. Template variants and when to use each

| Short name     | Identity                                          | Style | Use when |
| -------------- | ------------------------------------------------- | ----- | -------- |
| `ddvs`         | `RM.DomainDrivenVerticalSlices.Template`          | Clean Architecture (layered projects + Aspire) | Default / backward-compatible Clean Architecture app. |
| `ddvs-clean`   | `RM.DomainDrivenVerticalSlices.Template.Clean`    | Clean Architecture (identical source to `ddvs`) | You want an explicit "clean" short name; same generated output as `ddvs`. |
| `ddvs-modular` | `RM.DomainDrivenVerticalSlices.Template.Modular`  | Modular monolith, single-project Minimal API | LLM-friendly modular monolith organized by business module + vertical slice (API-only since 10.6.0). |

`ddvs` and `ddvs-clean` are **two template configs over the same source tree** (root
`src/` + `test/`). Any change to the Clean Architecture source affects both. `ddvs-modular`
is a **separate source tree** under `templates/ddvs-modular/`.

### Options per variant

| Option | Applies to | Values (default) | Effect |
| ------ | ---------- | ---------------- | ------ |
| `--ApiType` | `ddvs`, `ddvs-clean` | `Controller` (default), `MinimalApi` | Emit MVC controllers or Minimal API endpoints. Mutually exclusive source is excluded via template modifiers. |
| `--UiType`  | `ddvs`, `ddvs-clean` | `None` (default), `React` | Include the `*.UI.React` project (React 19 + Vite 6 + Tailwind v4). |
| `--UseAspire` | `ddvs-modular` | `true` (default), `false` | `false` excludes `*.AppHost` and `*.ServiceDefaults`. |

> The Clean Architecture template **always** includes Aspire (`AppHost` +
> `ServiceDefaults`). There is no `--UseAspire` on `ddvs`/`ddvs-clean`. Only
> `ddvs-modular` has `--UseAspire`. Conversely `--ApiType`/`--UiType` do **not** exist on
> `ddvs-modular` (it is API-only Minimal API since 10.6.0).

## 3. Architecture map

```
DomainDrivenVerticalSlices.Template/
├── .template.config/template.json            # 'ddvs'  config (source = repo root)
├── DomainDrivenVerticalSlices.Template.slnx  # Clean Architecture solution
├── src/                                       # Clean Architecture template source (ships)
│   ├── *.Domain/          # Entities, value objects, domain events, BaseEntity/BaseAuditableEntity
│   ├── *.Application/     # Commands, queries, DTOs, validators, pipeline behaviours
│   ├── *.Infrastructure/  # EF Core DbContext, interceptors, DatabaseInitializer
│   ├── *.WebApi/          # Controllers/ and Endpoints/ (one kept per --ApiType)
│   ├── *.Common/          # Custom lightweight mediator, Result/Error, ValueObject base
│   ├── *.ServiceDefaults/ # OpenTelemetry, health checks, service discovery, resilience
│   ├── *.AppHost/         # .NET Aspire orchestration
│   └── *.UI.React/         # React UI (only when --UiType React)
├── test/                                      # xUnit test projects (ship as template source)
│   └── *.{Common,Domain,Application,Infrastructure,WebApi}.Tests, *.IntegrationTests
├── templates/
│   ├── ddvs-clean/.template.config/template.json   # 'ddvs-clean' config (source = ../../)
│   └── ddvs-modular/                                # Modular monolith template (separate tree)
│       ├── .template.config/template.json          # 'ddvs-modular' config (--UseAspire)
│       ├── DomainDrivenVerticalSlices.Template.slnx # uses <!--#if (USE_ASPIRE)--> conditionals
│       ├── src/*.Api/       # single-project Minimal API
│       │   ├── Common/{Endpoints,Errors,Persistence}
│       │   └── Modules/Entity1/{Contracts,Domain,Endpoints,Features/<Slice>,Infrastructure}
│       ├── src/*.AppHost, *.ServiceDefaults        # excluded when --UseAspire false
│       └── tests/*.{UnitTests,IntegrationTests,ArchitectureTests}
├── Directory.Build.props / Directory.Packages.props  # central package mgmt + transitive pinning
├── global.json                                # SDK 10.0.102, rollForward latestFeature
└── .github/workflows/{build,codeql,package}.yml
```

`*` above stands for `DomainDrivenVerticalSlices.Template`, which is the template
`sourceName` and is renamed to the user's project name on generation.

## 4. Build, test, pack, smoke-test commands

Prerequisites: **.NET 10 SDK 10.0.102+** (`global.json`, `rollForward: latestFeature`),
PowerShell 7, Python 3.12+, Node.js 20.19+, 22.13+, or 24+ (React only), and NuGet CLI
(`nuget.exe` in the repo root or `nuget` with Mono, as appropriate for the host). Integration tests use
in-host SQLite and do not need Docker.

```bash
# Full source, dependency, package, and ten-configuration validation
pwsh -File scripts/validate-template.ps1
```

This shared script runs locally, in CI, and before publishing. It uses NuGet CLI with
`-NoDefaultExcludes` and checks that required dotfiles remain in the package.

NuGet licenses must permit use and redistribution in this MIT repository. Dependencies keep their own licenses; do not relabel them MIT. Review every resolved direct/transitive license and retain required notices. See `docs/DEPENDENCIES.md`.

## 4b. Cross-template parity rule (applies to every change)

**Every change must be evaluated against BOTH template trees** — the Clean Architecture tree
(root `src/`+`test/`, shipped as `ddvs` **and** `ddvs-clean`) and the modular tree
(`templates/ddvs-modular/`). They are separate source trees that share concepts, so a bug or
improvement in one usually has a counterpart in the other (example: the hardcoded
`"Entity1 API"` Swagger title existed in both trees and had to be fixed in both). For each
change, explicitly do one of:

- **Apply it to both trees** (dependency pins, packaging excludes, doc fixes, shared defects), or
- **Justify why it is single-tree** (e.g. mediator fixes are Clean-only because the modular
  tree deliberately has no mediator; React/npm work is Clean-only because modular has no UI).

Record that decision in the "Affected template variants" section of the final response
(§13 item 4) — never silently fix one tree and leave the twin defect in the other.

## 5. Rules for Clean Architecture template changes (`ddvs` / `ddvs-clean`)

- Source lives in root `src/` and `test/`. Changes affect **both** `ddvs` and `ddvs-clean`.
- Keep the dependency direction: `Domain` ← `Application` ← `Infrastructure`/`WebApi`;
  `Common` is shared. Domain must not depend on EF Core, ASP.NET, or Application.
- `--ApiType` excludes source via `template.json` modifiers. If you add/rename files under
  `WebApi/Controllers/**`, `WebApi/Endpoints/**`, `WebApi/Infrastructure/Endpoint*.cs`, or
  `WebApi.Tests/{Controllers,Endpoints}/**`, update the matching `exclude` lists in **both**
  `.template.config/template.json` and `templates/ddvs-clean/.template.config/template.json`.
- `--UiType None` excludes `src/*.UI.React/**`. Keep AppHost `Program.cs` React/Vite code
  behind `#if INCLUDE_REACT` (do not leave blank lines around `#if` — causes StyleCop SA1507).
- See the **`architecture-review`** skill for DDD/slice rules, **`add-module-or-slice`** for
  the new-slice checklist, and **`react-ui-development`** for UI work.

## 6. Rules for modular monolith template changes (`ddvs-modular`)

- Source lives in `templates/ddvs-modular/`. It is a **separate solution and package set**
  with its own `global.json`, `Directory.Build.props`, and `Directory.Packages.props`.
- Prefer **module-local vertical slices**: put a feature under
  `Modules/<Module>/Features/<Slice>/` with its `Request`/`Handler`/`Validator`/`Response`.
  Keep EF configuration inside the module (`Modules/<Module>/Infrastructure/`).
- The modular monolith favors local feature context over strict physical layering, which keeps
  features easier to navigate but makes architecture tests and team discipline important.
- Flow is explicit: **Endpoint → Validator → Handler → DbContext/Domain**. Do **not**
  introduce a mediator, generic repositories, or Unit of Work.
- Cross-module communication goes through `Contracts/` (e.g. integration events), not by
  reaching into another module's internals.
- Aspire projects (`*.AppHost`, `*.ServiceDefaults`) are gated by `--UseAspire`. If you add
  Aspire wiring, keep it behind the `<!--#if (USE_ASPIRE)-->` conditionals in the `.slnx`
  and any affected files, and verify `--UseAspire false` still generates + builds.
- Domain/integration events in the modular tree are **inert extension points** by design —
  no mediator or event pipeline exists; do not add hidden dispatch machinery (see the
  Entity1 module README).
- Use the **`add-module-or-slice`** skill for the full new-module checklist.

## 7. Rules for `.template.config` changes

- There are **three** configs: root `.template.config/template.json` (`ddvs`),
  `templates/ddvs-clean/.template.config/template.json` (`ddvs-clean`), and
  `templates/ddvs-modular/.template.config/template.json` (`ddvs-modular`).
- `ddvs` and `ddvs-clean` must stay in lockstep (same symbols, choices, modifiers, excludes)
  — they render the same source. Change both together.
- Do **not** change `identity`, `shortName`, or `sourceName` without treating it as a
  breaking change (see §8, §12). `sourceName` is `DomainDrivenVerticalSlices.Template` in
  all three and must match the real project/folder names, or renaming breaks.
- After any config change, run the smoke matrix and the generated-output assertions
  (`dotnet-template-validation` skill). A wrong `exclude` silently ships extra files or
  omits needed ones.
- Use the **`template-option-authoring`** skill when adding or changing template options,
  choices, or defaults.

## 8. Rules for package / release changes

- Version lives in `DomainDrivenVerticalSlices.Template.nuspec` (`<version>`). Bump it with
  every published change and update `<releaseNotes>` in the same edit.
- Pack **only** with `nuget.exe pack ... -NoDefaultExcludes`. `dotnet pack` is wrong here.
- The `<files>` `exclude` list in the `.nuspec` and the `sources.exclude` lists in the
  template configs must both keep dev-only files out of shipped content. That covers:
  workflows, instructions, artifacts, `nuget.exe`, `*.nupkg`, NuGet `packages.lock.json`,
  build/test outputs (`coverage/`, `dist/`, `*.opencover.xml`, `TestResults/`), the root
  `.gitignore`/`.prettierignore`/`icon.png`/`LICENSE`/`CODE_OF_CONDUCT.md`, the root `.github/copilot-instructions.md`, **and all repo-maintainer/AI guidance**:
  `.claude/**`, `CLAUDE.md`, `AGENTS.md`, `scripts/**`, `CHANGELOG.md`, maintainer docs under `docs/`,
  `*.code-workspace`. If you add a new dev-only file (e.g. another doc under `docs/`), add
  it to the `.nuspec` exclude **and** to both `ddvs`/`ddvs-clean` template configs in the
  same change.
- Distinguish two doc classes: **repo-dev docs** (above — never ship) vs **generated-project
  docs** that ship on purpose: `README.md` + `docs/screenshots/**`, per-project `README.md`s
  (including the modular `Modules/<Module>/README.md`), and the React project's `.gitignore`.
  Do not exclude the shipped class; keep its content accurate for generated projects.
- **Neither tree ships `.github/**` or repo-only docs into generated projects.** The modular
  template config and `.nuspec` keep broad excludes for `.github/**` and `docs/**`; retain
  them so future maintainer references cannot leak into generated projects. The Clean tree
  also excludes its `.github/**` and maintainer docs.
- `nuget.exe` packs **from disk**, so untracked/gitignored files ship in local packs but not
  in CI packs. Product content must be tracked in git; pack releases only from a clean
  checkout (or CI) so the package is reproducible.
- Keep `global.json`, `Directory.Packages.props`, and CI SDK versions aligned (see §11).
- See **`release-notes-and-commit`** and **`docs/RELEASE_PROCESS.md`**.

## 9. Rules for UI changes (React)

- The React project is `src/DomainDrivenVerticalSlices.Template.UI.React/` (React 19, Vite 6,
  Tailwind CSS v4, Vitest + MSW). It ships only under `--UiType React`.
- A `package-lock.json` exists — keep it in sync and prefer `npm ci` for reproducible installs.
- Aspire integration uses `AddViteApp(...)` and `vite.config.js` reads
  `services__webapi__https__0` for dynamic ports — do not hardcode ports.
- Address npm audit findings via `Directory.Packages.props`-equivalent discipline: pin/upgrade
  in `package.json` + lockfile, and record CVEs in release notes (see 10.4.0/10.5.0 history).
- Use the **`react-ui-development`** skill for the dev workflow, testing, and the
  Aspire/Vite dynamic-port contract.

## 10. Rules for tests and generated-template validation

- Both template source trees must build with **0 warnings** (`TreatWarningsAsErrors=true`).
- Run the full unit/integration/architecture suites for the tree you touched.
- A source change is not "done" until a **generated** project from the affected variant
  builds and tests green. Static build of the template source is necessary but not sufficient.
- Use the `dotnet-template-validation` skill's smoke matrix + assertions:
  no `DomainDrivenVerticalSlices.Template` string leaks into generated output, no
  `.template.config` folder leaks, no repo-dev files leak (`CLAUDE.md`, `.claude/`,
  `CHANGELOG.md`, `docs/ROADMAP.md`, `docs/RELEASE_PROCESS.md`, `*.code-workspace`,
  `packages.lock.json`), and `--UseAspire false` omits AppHost/ServiceDefaults.

### Comprehensive test gate (end of development)

When development on a change is complete, follow the user's requested test scope. If the user
has authorized the full matrix, run the shared script; otherwise agree on a targeted scope.

- **No** → run the *targeted* validation only: the variant(s) and option(s) you touched
  (see §10 bullets and the `dotnet-template-validation` skill).
- **Full matrix** → run `pwsh -File scripts/validate-template.ps1`. It generates, builds, and
  tests every option combination, builds/tests each React project, and checks generated output.

| Variant | Full matrix to generate + build + test |
| ------- | -------------------------------------- |
| `ddvs` | `--ApiType {Controller,MinimalApi}` × `--UiType {None,React}` = 4 combos |
| `ddvs-clean` | same 4 combos (must match `ddvs` output — parity) |
| `ddvs-modular` | `--UseAspire {true,false}` = 2 combos |

That is ten generated projects; the four React variants additionally build and test the UI.

## 11. Security and dependency expectations

- **Central Package Management** is on (`ManagePackageVersionsCentrally=true`) with
  `CentralPackageTransitivePinningEnabled=true`. Add/adjust versions in
  `Directory.Packages.props` (root and `templates/ddvs-modular/`), never inline in `.csproj`.
- Lock files are enabled in both source trees. CI restores in locked mode; AppHost projects
  opt out because Aspire injects platform-specific packages. Generated projects omit maintainer
  lock files and need an unlocked initial restore. Refresh locks intentionally after version changes.
- Keep CI `setup-dotnet` versions aligned with `global.json` (10.0.x band).
- Run the **`dependency-security-review`** skill when touching versions, workflows, Docker,
  npm, or NuGet. Do not suppress vulnerability warnings without a documented reason.

## 12. Do NOT do without explicit permission

- Publish to NuGet (do not run `nuget push` or the `package.yml` workflow).
- Push commits or open/merge PRs.
- Create git tags or GitHub releases.
- Delete major folders (`src/`, `test/`, `templates/`, `.github/`, `artifacts/`, `nupkgs/`).
- Change public template names or short names (`ddvs`, `ddvs-clean`, `ddvs-modular`),
  identities, or `sourceName`.
- Introduce breaking changes (renamed/removed options, changed defaults, changed generated
  structure) **without** documenting migration notes.

## 13. Every meaningful change must include

1. **Validation summary** — exact commands run and their results (or the recorded failure +
   likely cause + next action if the environment blocked a command).
2. **Release notes** — in the format defined by the `release-notes-and-commit` skill.
3. **Proposed conventional commit** — `<type>(<scope>): <summary>` (types: `feat`, `fix`,
   `docs`, `test`, `refactor`, `build`, `ci`, `chore`, `security`).
4. **Affected template variants** — which of `ddvs` / `ddvs-clean` / `ddvs-modular` (and
   which options) are impacted, **including the §4b parity statement**: either "applied to
   both trees" or the reason the change is legitimately single-tree.
5. **Migration notes** — required whenever behavior, options, names, or generated output
   change; otherwise state "None".
6. **Comprehensive-test decision** — follow §10 and the user's requested scope; record whether
   the full matrix or targeted validation was run.

---

## Skills in this repo (`.claude/skills/`)

| Skill | Use when |
| ----- | -------- |
| `dotnet-template-validation` | `.template.config`, `.nuspec`, template source, solution, or packaging changes. |
| `architecture-review` | Domain/application/infrastructure/WebApi/modular/pipeline/persistence/event changes. |
| `release-notes-and-commit` | End of every meaningful change. |
| `dependency-security-review` | Package versions, workflows, Docker, npm, NuGet, CPM, or CI changes. |
| `documentation-consistency` | README, docs, template configs, `.nuspec` metadata, or release notes changes. |
| `template-option-authoring` | Adding/renaming/changing a template option, its choices, or defaults in any `template.json`. |
| `add-module-or-slice` | Adding a feature slice (Clean tree) or a business module/slice (modular tree). |
| `react-ui-development` | Any change under `*.UI.React/` or the Aspire wiring that hosts it. |

See also `docs/ROADMAP.md`, `docs/RELEASE_PROCESS.md`, `CHANGELOG.md`, and the root
`.github/copilot-instructions.md` entrypoint.
