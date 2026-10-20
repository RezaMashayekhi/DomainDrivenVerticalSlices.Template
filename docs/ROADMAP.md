# Roadmap

This roadmap describes current source state and planned direction. Later version sections are
candidates, not promises.

## Current baseline

Package source `RM.DomainDrivenVerticalSlices.Template` is at **10.8.0**, targeting **.NET 10**.

- **`ddvs` / `ddvs-clean`** — Clean Architecture template (root `src/` + `test/`), with
  `--ApiType Controller|MinimalApi`, `--UiType None|React`, Aspire always included.
- **`ddvs-modular`** — modular monolith, single-project Minimal API, SQLite, `--UseAspire`
  (default true). API-only as of 10.6.0/10.7.0.
- Central Package Management + transitive pinning + lock files in both trees; CI uses the
  shared validator to build/test both source trees, audit licenses and vulnerabilities, pack
  once, and generate/build/test all ten configurations from the tested package.
- 10.7.0 was a security/correctness release: transitive NuGet pins + npm audit fixes, the
  domain-event dispatch fix, Debug-only payload logging, and packaging-leak/reproducibility
  fixes (see `CHANGELOG.md`).

## 10.8.0 implemented

- **Validation and release artifact** — one local/CI script runs the full source and ten
  generated-configuration matrix; package workflow publishes the exact validated package
  after checking commit identity and SHA-256.
- **Dependency and license review** — added resolved direct/transitive NuGet license and
  vulnerability gates, exact-version evidence for interpreted licenses, and refreshed lock
  files. Kept dependencies whose license permits repo use and redistribution, including
  permissive Apache/BSD terms; no third-party code is relabeled MIT. Refreshed the React/npm
  graph, moved to React Router 7 to resolve its advisories, and recorded npm license obligations.
- **Modular persistence** — ownership and registration moved to the API/common persistence
  boundary; SQLite uniqueness races return a conflict.
- **Clean Architecture integration tests** — replaced the unused Testcontainers factory with
  an in-host SQLite database.
- **Architecture checks** — module boundaries now inspect method-body IL references as well
  as signatures and cover domain, feature, and infrastructure boundaries.
- **Clean API parity and Result values** — shared API error mapping and endpoint behavior;
  `Result<T>` supports valid default values and throws `InvalidOperationException` for missing
  values.
- Added maintainer documentation, migration notes, agent-entrypoint adapters, modular
  `.editorconfig`, and modular lock files. Removed unused Testcontainers dependencies.

Deferred candidates remain provider selection for modular persistence and a modular React
option; neither is part of 10.8.0.

## 10.9.0 candidates

- **Optional authentication/authorization choices** — template options for JWT/OIDC-based
  auth wiring in both templates.
- **OpenAPI and API versioning improvements** — richer OpenAPI metadata and an API-versioning
  option for generated APIs.
- **Better observability defaults** — expand the OpenTelemetry/health-check story and document
  exporters.
- **Mediator internals** — cache handler invokers (compiled delegates keyed by request type)
  instead of per-call reflection, and unwrap `TargetInvocationException` so
  `UnhandledExceptionBehaviour` logs the real exception.
- **Optional AI-agent guide for generated projects** — *if* pursued, ship any agent guide
  (`CLAUDE.md`/`copilot-instructions.md`) only behind an explicit opt-in switch
  (e.g. `--IncludeAgentDocs`), default off. Generated projects are intentionally kept clean:
  the template must not push its own AI instructions or design ADRs into a user's project by
  default so generated projects do not inherit the template's own AI guidance or reference docs.

## 11.0.0 candidates

- **Breaking cleanups** that would change generated structure, defaults, or option names —
  each must land with explicit migration notes.
- **Modular placeholder rename** (e.g. `Entity1` → a domain-flavored sample) if it changes
  generated file/type names — breaking, needs migration notes.
- **.NET major upgrade** when the next LTS/current band ships.

## Backlog

- Docker Compose support for generated solutions (non-Aspire runtime option).
- Modular production-readiness guidance: `appsettings.json` has no `ConnectionStrings` and
  schema creation (`EnsureCreatedAsync`) runs only in Development — document EF migrations /
  production DB setup.
- Scheduled npm dependency review (NuGet vulnerabilities are checked by the CI validation gate).
- Repo housekeeping: prune stale root artifacts (`RM.DomainDrivenVerticalSlices.Template.10.2.1.nupkg`,
  old `nupkgs/`/`artifacts/` contents) and reconsider committing `nuget.exe` — **requires
  maintainer approval; not done automatically** (see `docs/MAINTAINER_GUIDE.md` §12).
- Documentation-consistency CI check: grep version/name/short-name/prerequisite strings across
  README, `.nuspec`, all three `template.json` files, and docs, and fail on drift.

## Non-goals

- Turning the templates into a specific product/app rather than a starting point.
- Adding heavy frameworks (a full mediator library, generic repositories, Unit of Work) to
  the modular template — it is intentionally explicit and mediator-free.
- Supporting non-.NET backends.
- Guaranteeing dates or committing to any candidate above without a corresponding release.

## Release decision checklist

Before promoting a candidate into a release:

- [ ] Change validated via `scripts/validate-template.ps1` (all ten variants for release).
- [ ] `Directory.Packages.props` / lock files updated; `dotnet list package --vulnerable` clean.
- [ ] `.nuspec` `<version>` bumped and `<releaseNotes>` written.
- [ ] `CHANGELOG.md` `Unreleased` moved to the new version section.
- [ ] README/docs/config consistency verified (`documentation-consistency` skill).
- [ ] Breaking changes carry migration notes.
- [ ] Proposed conventional commit prepared; **no** publish/tag without maintainer approval.
