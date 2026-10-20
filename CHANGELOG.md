# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this
project adheres to a semantic-ish versioning scheme for template packages (see
`docs/RELEASE_PROCESS.md`). The authoritative package version is `<version>` in
`DomainDrivenVerticalSlices.Template.nuspec`. Historical entries below (10.3.0–10.6.0) are
derived from the `.nuspec` release notes and README history already present in the repository.

## [Unreleased]

## [10.8.0] - 2026-10-20

### Added

- Shared source/package/license/vulnerability validation and complete ten-configuration
  generated-template validation, reused by CI and the publish workflow. Publishing checks the
  tested artifact's commit and SHA-256 before upload.
- Exact-version NuGet license evidence and dependency compatibility guidance.
- Migration notes for the `Result<TValue>` behavior, SQLite test host, and modular persistence.

### Changed

- Updated the generated React UI dependency graph to current compatible releases and moved
  React Router to 7.18.4 to clear all npm audit advisories. The UI uses unchanged declarative
  routing APIs; React UI Node requirements now identify supported LTS release lines. Pinned
  transitive `source-map-js` 1.2.2 for GHSA-68fv-2mgg-jv7q and added a fail-closed online npm
  audit to the shared validation gate.
- Updated NuGet dependencies and refreshed lock files in both template trees.
- Clean API error handling is consistent across controllers and Minimal API endpoints.
- Modular persistence ownership is explicit; architecture checks cover IL-level dependencies;
  SQLite uniqueness races return HTTP 409.
- Clean Architecture integration tests use in-host SQLite; removed the unused Testcontainers
  dependency and idle container factory.
- Consolidated modular architecture guidance in the maintainer guide and architecture-review
  skill; removed duplicate reference pages and pointer-only AI instruction files.
- Removed stale archive stubs and an obsolete historical walkthrough; release history remains
  in this changelog.

### Fixed

- Restored missing bundled Tailwind WebAssembly dependency entries in the React lock file
  so clean npm installs can validate the complete dependency tree.
- Matched integration-test environment names to the exact settings-file casing, preventing
  React-enabled test hosts from failing CORS configuration on case-sensitive filesystems.
- `Result<TValue>` success accepts default values such as `0` and `false`; unavailable values
  throw `InvalidOperationException`.

### Validation

- In the local validation run, the shared validator completed on 2026-10-18 under `CI=true`: 217 Clean Architecture
  source tests and 23 modular source tests passed; all ten generated configurations built and
  passed tests. The four generated React projects each passed build and 32 UI tests. NuGet
  license review covered 182 resolved packages with no review items, both NuGet vulnerability
  reports were clean, and online npm audit reported zero advisories.

## [10.7.0] - 2026-07-03

### Added

- `CLAUDE.md` — repository guidance for Claude Code / AI agents (variants, architecture map,
  build/test/pack/smoke commands, change rules, "do not do without permission" list), including
  an end-of-development "comprehensive test gate": ask the developer whether a full
  all-variations build+test (including the React UI) is needed.
- `.claude/skills/` — eight project-local skills: `dotnet-template-validation`,
  `architecture-review`, `release-notes-and-commit`, `dependency-security-review`,
  `documentation-consistency`, `template-option-authoring`, `add-module-or-slice`, and
  `react-ui-development`.
- `docs/ROADMAP.md`, `docs/RELEASE_PROCESS.md`, and `CHANGELOG.md` (this file).
- Unit tests for the custom mediator (`MediatorTests`) — including a regression test that
  publishes a notification through a base-typed `INotification` reference.
- Tests for both EF Core interceptors (`DispatchDomainEventsInterceptorTests`,
  `AuditableEntityInterceptorTests`), covering after-save dispatch, event clearing,
  cancellation, and audit-field population.
- Generated `--UiType React` projects now receive the React project's `.gitignore`
  (previously stripped from the package by a blanket exclude), extended to ignore Vite's
  `dist/` output.

### Changed

- **Domain events now follow a single dispatch path** in `ddvs`/`ddvs-clean`:
  `CreateEntity1CommandHandler` no longer manually publishes `Entity1CreatedEvent`; the event
  raised by `Entity1.Create` is dispatched (exactly once) by `DispatchDomainEventsInterceptor`
  after a successful save. *Migration note:* if you added handlers relying on the manual
  publish, they keep working — events now arrive via the interceptor instead, after
  persistence; the handler no longer takes an `IPublisher` constructor parameter.
- **Both templates**: the Swagger document title now uses the template `sourceName` (renamed
  to your project name on generation) instead of the hardcoded "Entity1 API" — fixed in the
  Clean tree (`ServiceExtensions`/`AppExtensions`) and the modular tree (`Program.cs`).
- `LoggingBehaviour` (`ddvs`/`ddvs-clean`) no longer logs request/response payload properties
  at `Information`: payload detail (with the sensitive-name denylist) moved to `Debug` and is
  computed only when `Debug` is enabled. `Information` keeps request name, outcome, and error
  details. `PerformanceBehaviour` and `UnhandledExceptionBehaviour` likewise no longer embed
  the full request object (`{@Request}`) in their `Warning`/`Error` messages — only the
  request name. *Migration note:* to restore the old verbosity, set the log level for the
  `LoggingBehaviour` category to `Debug` (integration tests do this via
  `appsettings.integrationtest.json`); note the denylist is name-based and non-exhaustive,
  so payload logging remains opt-in by design.
- `ddvs-modular`: documented that `Entity1CreatedDomainEvent` and
  `Entity1CreatedIntegrationEvent` are deliberately inert extension points (no hidden event
  pipeline exists in the modular template).
- React `package.json`/`package-lock.json` project version aligned with the package version.
- Integration-test factories re-attach the production EF Core interceptors when they replace
  the `DbContext` registration, so audit fields and domain-event dispatch are exercised by
  integration tests.
- CI (`build.yml`): upload the packed `.nupkg` as a build artifact; use `npm ci` (lockfile
  present) instead of `npm install` for deterministic React installs.
- CI (`codeql.yml`): use `npm ci` for the JavaScript analysis install.
- CI (`package.yml`): run `apt-get update` before installing Mono; bump `actions/checkout` v3 → v4.
- README: absolute GitHub URLs for maintainer-doc and LICENSE links (they now work on
  nuget.org and are correct even though the files no longer ship), a note that maintainer
  docs are repo-only, a 10.5.0 security section closing the "What's New" gap, and a fixed
  `<br>`-joined feature bullet.
- Shipped Domain README corrected: `Entity1` inherits `BaseEntity` (not
  `BaseAuditableEntity`), and audit field names now match the real class
  (`Created`/`CreatedBy`/`LastModified`/`LastModifiedBy`).
- **Updated NuGet dependencies to latest-in-major across both trees**: EF Core +
  `Microsoft.Extensions.*` + `AspNetCore.Mvc.Testing` 10.0.7 → 10.0.9, Http.Resilience +
  ServiceDiscovery 10.5.0 → 10.7.0, Aspire 13.2.4 → 13.4.6, OpenTelemetry 1.15.x → 1.16.0
  (`Instrumentation.Runtime` held at 1.15.1 — no 1.16.0 published; compatible with Api 1.16.0),
  `Swashbuckle.AspNetCore` 10.1.7 → 10.2.3, `Microsoft.NET.Test.Sdk` 18.5.1 → 18.7.0,
  `coverlet.*` 10.0.0 → 10.0.1, `Testcontainers` 4.11.0 → 4.13.0. Both trees build 0-warning
  and all 214 root + 20 modular tests pass; `dotnet list package --vulnerable` is clean in
  both trees; all four generated variants (`ddvs`, `ddvs --UiType React`, `ddvs-modular`,
  `ddvs-modular --UseAspire false`) build 0-warning/0-error on a fresh restore and pass.

### Fixed

- **Domain events were silently dropped** (`ddvs`/`ddvs-clean`): `Mediator.Publish` resolved
  handlers by the compile-time generic type, so events dispatched from
  `DispatchDomainEventsInterceptor` (statically typed `INotification`) never reached any
  registered handler. `Publish` now resolves handlers by the notification's runtime type.
  The sample previously appeared to work only because the Create handler published a second
  copy of the event manually.
- `DispatchDomainEventsInterceptor` dispatched **before** `SaveChanges` (contradicting its
  own documentation and `BaseEntity`'s), ran handlers on unsaved state, and dropped the
  cancellation token. It now dispatches in `SavedChanges`/`SavedChangesAsync` — after
  successful persistence — and forwards the cancellation token.
- **Packaging: repo-dev and AI-maintainer files no longer ship.** Added `.claude/**`,
  `CLAUDE.md`, `CHANGELOG.md`, `docs/ROADMAP.md`, `docs/RELEASE_PROCESS.md`,
  `*.code-workspace`, and the gitignored `.github/copilot-instructions.md` to the `.nuspec`
  excludes and to the `ddvs`/`ddvs-clean` template config excludes. (Intentionally shipped
  docs are unaffected: `README.md`, `docs/screenshots/**`, and per-project READMEs including
  the modular `Modules/<Module>/README.md`.)
- **Modular template no longer copies its `.github/` Copilot instructions or
  `docs/architecture/` ADRs into generated projects.** 10.6.0 generated modular projects
  received the template's own `.github/copilot-instructions.md`, `.github/instructions/*.md`,
  and `docs/architecture/*.md`, so a new project inherited the template's AI instructions and
  design decisions as if they were its own. The modular `template.json` now excludes
  `.github/**` and `docs/**`, and the `.nuspec` excludes `templates/ddvs-modular/.github/**`
  and `templates/ddvs-modular/docs/**`; the files remain in the repository as
  maintainer/reference docs. This brings the modular tree in line with the Clean tree, which
  already keeps its `.github/**` and dev `docs/` out of generated projects. *Migration note:*
  projects generated from the next version no longer contain these files; delete them from
  any project generated with 10.6.0 if you did not intend to keep them.
- Packaging: the `*.code-workspace` exclusion also fixes a pre-existing leak — 10.6.0 shipped
  `DomainDrivenVerticalSlices.Template.code-workspace`, which exposed maintainer-local folder
  paths and generated a dangling `<ProjectName>.code-workspace` in new projects.
  *Migration note:* projects generated from the next version simply no longer contain that
  file; it was never functional.
- Packaging: locally packed packages no longer diverge from CI packs — gitignored build/test
  outputs (`coverage/`, `dist/`, `*.opencover.xml`, `TestResults/`) and NuGet
  `packages.lock.json` files are excluded from the `.nupkg`; the React `package-lock.json`
  still ships (required for `npm ci`). The `ddvs`/`ddvs-clean` template configs now mirror
  the nuspec's dev-only excludes so local-folder installs generate the same output as
  package installs, and the modular config excludes `.vs/**` and `*.user`.
- Restored `templates/ddvs-modular/docs/architecture/` (overview, module-boundaries, testing,
  ADR-0001) from the published 10.6.0 package: the four files were never committed to git and
  had been lost from the working tree, so the repo could not reproduce its own published
  package. They are now git-tracked as maintainer/reference docs and no longer ship into
  generated projects (see the modular `.github/`/`docs/` exclusion above).
- CI (`codeql.yml`): align `setup-dotnet` to `10.0.x` (was hard-pinned `10.0.100`, lower than
  `global.json`'s `10.0.102` requirement) to avoid SDK-resolution failures.

### Security

- Pinned three vulnerable transitive packages via Central Package Management transitive
  pinning, in **both** source trees (`Directory.Packages.props` root + modular). Without
  these pins, **newly generated projects fail to build** — NuGet audit raises NU1902/NU1903
  on fresh restore and `TreatWarningsAsErrors` turns them into errors:
  - `MessagePack` 2.5.192 → **2.5.302** (GHSA-hv8m-jj95-wg3x, High — LZ4 decompression
    out-of-bounds read; several related moderate advisories).
  - `Microsoft.OpenApi` 2.4.1 → **2.7.5** (GHSA-v5pm-xwqc-g5wc, High — uncontrolled recursion
    / stack overflow on circular schema references).
  - `SQLitePCLRaw.lib.e_sqlite3` 2.1.11 → **3.50.3** (GHSA-2m69-gcr7-jv3q / CVE-2025-6965,
    High — SQLite memory corruption; the 3.50.x line bundles the fixed native SQLite ≥ 3.50.2).
  NuGet lock files regenerated for the root tree; all 214 root + 20 modular tests pass with
  the pins.
- React UI: resolved all 9 `npm audit` findings (2 critical, 2 high, 4 moderate, 1 low) via a
  lockfile-only refresh (`npm audit fix`; `package.json` ranges unchanged; audit now clean):
  - `vitest` → 4.1.9 and `@vitest/coverage-v8` (GHSA-5xrq-8626-4rwp, **Critical** — Vitest UI
    server arbitrary file read/execute).
  - `vite` → 6.4.3 (GHSA-v6wh-96g9-6wx3 — launch-editor NTLMv2 hash disclosure;
    GHSA-fx2h-pf6j-xcff — `server.fs.deny` bypass; both High).
  - `ws` → 8.21.0 (GHSA-58qx-3vcg-4xpx — uninitialized memory disclosure;
    GHSA-96hv-2xvq-fx4p — memory-exhaustion DoS; both High).
  - `react-router`/`react-router-dom` → 6.30.4 (GHSA-2j2x-hqr9-3h42 — open redirect, Moderate).
  - `js-yaml` → 4.3.0 (GHSA-h67p-54hq-rp68, Moderate); `brace-expansion` → 5.0.7
    (GHSA-jxxr-4gwj-5jf2, Moderate); `@babel/core` → 7.29.7 (GHSA-4x5r-pxfx-6jf8, Low).
  UI verified after the refresh: `npm ci`, `npm run build`, and all 32 Vitest tests pass.

## [10.6.0] - 2026-05-25

### Added

- `ddvs-modular`: an LLM-friendly modular monolith API template organized by business module
  and vertical slice.
- `ddvs-clean`: an explicit compatibility short name for the existing Clean Architecture template.
- Modular: single-project Minimal API backend with an `Entity1` module sample.
- Modular: direct Endpoint → Validator → Handler → DbContext/Domain flow (no mediator, generic
  repositories, or Unit of Work).
- Modular: Result/Error/ProblemDetails mapping for expected business failures.
- Modular: SQLite persistence with EF Core configuration kept inside the `Entity1` module.
- Modular: optional Aspire AppHost and ServiceDefaults via `--UseAspire` (default `true`).
- Modular: generated unit, integration, and architecture tests.

### Changed

- Preserved `ddvs` as the backward-compatible Clean Architecture template short name.

## [10.5.0] - 2026-04-29

### Changed

- Upgraded EF Core 10.0.5 → 10.0.7.
- Upgraded Aspire 13.2.0 → 13.2.4.
- Upgraded `Microsoft.Extensions.Http.Resilience` + `ServiceDiscovery` 10.4.0 → 10.5.0.
- Upgraded `Microsoft.NET.Test.Sdk` 18.3.0 → 18.5.1.
- Upgraded `coverlet.collector` + `coverlet.msbuild` 8.0.1 → 10.0.0.
- Enabled NuGet transitive pinning for deterministic transitive dependency resolution.

### Security

- Fixed three OpenTelemetry DoS vulnerabilities (GHSA-g94r-2vxg-569j / CVE-2026-40894,
  GHSA-q834-8qmm-v933 / CVE-2026-40182, GHSA-mr8r-92fq-pj8p / CVE-2026-40891): upgraded
  `OpenTelemetry.Exporter.OpenTelemetryProtocol` + `Extensions.Hosting` to 1.15.3 and pinned
  transitive `OpenTelemetry.Api` to 1.15.3 via CentralPackageTransitivePinning.
- Fixed two Vite dev-server vulnerabilities (GHSA-p9ff-h696-f583 / CVE-2026-39363,
  GHSA-4w7w-66w2-5vf9 / CVE-2026-39365) by upgrading Vite to 6.4.2.
- Fixed a PostCSS XSS vulnerability (GHSA-qx2v-qp2m-jg93) by upgrading PostCSS to 8.5.12.

## [10.4.0] - 2026-03-27

### Added

- API style choice: traditional MVC controllers (`--ApiType Controller`, default) or Minimal
  API endpoints (`--ApiType MinimalApi`).
- Consistent `/api/{Entity}` routes across both API styles.

### Changed

- Minimal API endpoints aligned with controller response patterns for integration-test
  compatibility.

### Security

- Fixed a minimatch ReDoS vulnerability (CVE-2026-27903) in React UI dependencies.
- Updated transitive dependencies (ajv, flatted, rollup) to resolve additional vulnerabilities.

## [10.3.0] - 2026-02-07

### Added

- .NET Aspire 13.1 AppHost for orchestrated startup.
- ServiceDefaults with OpenTelemetry, health checks, service discovery, and resilience.
- React UI integration via `AddViteApp()` when `--UiType React`.
- `BaseEntity` (domain event collection) and `BaseAuditableEntity` (automatic audit fields).
- `AuditableEntityInterceptor` and `DispatchDomainEventsInterceptor` EF Core interceptors.
- `IUser`/`CurrentUser` abstraction for user context.
- `PerformanceBehaviour` and `UnhandledExceptionBehaviour` pipeline behaviours.
- `Entity1Endpoints` Minimal API alternative and `EndpointGroupBase` infrastructure.
- `TestcontainersWebApplicationFactory` for integration tests; MSW for React API mocking.

### Changed

- Migrated to Tailwind CSS v4 with dark mode support.
- Added Headless UI modals and Heroicons; consolidated Add/Edit into a unified `Entity1Form`.

[Unreleased]: https://github.com/RezaMashayekhi/DomainDrivenVerticalSlices.Template/compare/v10.8.0...HEAD
[10.8.0]: https://www.nuget.org/packages/RM.DomainDrivenVerticalSlices.Template/10.8.0
[10.7.0]: https://www.nuget.org/packages/RM.DomainDrivenVerticalSlices.Template/10.7.0
[10.6.0]: https://www.nuget.org/packages/RM.DomainDrivenVerticalSlices.Template/10.6.0
[10.5.0]: https://www.nuget.org/packages/RM.DomainDrivenVerticalSlices.Template/10.5.0
[10.4.0]: https://www.nuget.org/packages/RM.DomainDrivenVerticalSlices.Template/10.4.0
[10.3.0]: https://www.nuget.org/packages/RM.DomainDrivenVerticalSlices.Template/10.3.0
