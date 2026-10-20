---
name: architecture-review
description: Use whenever changing domain, application, infrastructure, Web API, modular modules, pipeline behaviours, persistence, domain events, or the generated architecture of any template variant. Enforces DDD, Clean Architecture, and Vertical Slice rules so generated code stays consistent and navigable.
---

# Architecture Review

This code ships to users as a starting point. Keep it exemplary, consistent, and simple
enough for both humans and AI agents to extend by copying an existing slice.

## Two architectures, one repo — pick the right rules

- **Clean Architecture** (`ddvs` / `ddvs-clean`): root `src/` + `test/`. Layered projects.
- **Modular monolith** (`ddvs-modular`): `templates/ddvs-modular/`. Single Minimal API project
  organized by module and slice.

Apply the section matching the tree you are editing — **and always check the other tree**
(docs/MAINTAINER_GUIDE.md §4b): a defect or improvement in one tree usually has a counterpart in the other
(shared concepts: Result/Error, Entity1 sample, Swagger setup, EF configuration, docs).
Either apply the change to both trees or record why it is legitimately single-tree
(no mediator and no UI exist in the modular tree; no module registration exists in the Clean
tree). Never silently fix one tree only.

## Clean Architecture rules (`ddvs` / `ddvs-clean`)

- Dependency direction only inward: `Domain` ← `Application` ← `Infrastructure`/`WebApi`.
  `Common` is shared kernel. **Domain references nothing outward.**
- Do **not** leak EF Core, ASP.NET, or persistence types into `Domain` or `Application`
  contracts. EF Core lives in `*.Infrastructure` (DbContext, interceptors, configurations).
- CQRS goes through the **custom lightweight mediator** in `*.Common` — do not add MediatR
  or a second mediator. Commands and queries stay explicit and separate.
- Business outcomes use the **Result/Error pattern** (`*.Common`), not exceptions for
  expected failures.
- Cross-cutting concerns belong in **pipeline behaviours** (`*.Application`): `Validation`,
  `Logging`, `Performance`, `UnhandledException`. Add cross-cutting logic there or in
  infrastructure — not sprinkled into handlers.
- Audit/eventing is centralized: `BaseEntity` (domain events), `BaseAuditableEntity` (audit
  fields), and EF Core interceptors (`AuditableEntityInterceptor`,
  `DispatchDomainEventsInterceptor`). Reuse them; don't reimplement per entity.
- **Domain events have exactly one dispatch path**: raise them on the entity with
  `AddDomainEvent(...)`; `DispatchDomainEventsInterceptor` publishes them **after** a
  successful save (`SavedChanges`). Never also `Publish` the same event manually from a
  handler — it would fire twice. (`Mediator.Publish` resolves handlers by the notification's
  runtime type; keep it that way.)
- Persistence access goes through the per-aggregate repository (`IEntity1Repository`) +
  `IUnitOfWork` abstractions that already exist — extend that pattern per aggregate; do not
  introduce a *generic* `IRepository<T>`.

## Vertical Slice rules

- Organize by **feature/business capability**, not by technical layer folders. Preserve the
  existing slice layout; add a feature by copying a sibling slice, not by inventing a new shape.
- Keep a command/query's request, handler, validator, and response together for that slice.
- Keep command/query behaviour explicit — no hidden conventions or reflection magic beyond
  what the existing mediator/DI already does.

## DDD aggregate / entity / value-object rules

- **No anemic domain models.** Put invariants and state transitions on the entity/aggregate,
  behind methods — not in handlers or services. Handlers orchestrate; the domain decides.
- Value objects are immutable and equality-by-value (`ValueObject` base in `*.Common` /
  modular `Domain/ValueObjects/`). Validate invariants in construction/factory methods.
- Raise **domain events** from the aggregate for in-process side effects; use integration
  event **contracts** for cross-boundary/cross-module signals.
- Keep aggregate boundaries small; reference other aggregates by id.

## Modular monolith rules (`ddvs-modular`)

- Layout per module: `Modules/<Module>/{Contracts, Domain, Endpoints, Features/<Slice>, Infrastructure}`
  plus `<Module>Module.cs` for registration. Follow the `Entity1` module as the reference.
- Flow is explicit and mediator-free: **Endpoint → Validator → Handler → DbContext/Domain**.
  Do **not** add a mediator, generic repositories, or Unit of Work.
- Keep slices **module-local** with **minimal coupling**. A module must not reach into
  another module's `Domain`, `Features`, or `Infrastructure`. Cross-module communication goes
  through `Contracts/` (e.g. `Entity1CreatedIntegrationEvent`).
- Keep EF Core configuration inside the owning module (`Modules/<Module>/Infrastructure/
  <Entity>Configuration.cs`); the shared `AppDbContext` (in `Common/Persistence/`) composes them.
- Use the module's `Common/Errors` `Result`/`Error` + `ResultExtensions` to map expected
  failures to `ProblemDetails`.

## Test expectations

- Every behavioural change gets/updates a unit test in the matching test project.
- Modular changes must keep the **ArchitectureTests** green (they encode module boundary and
  dependency rules). If you intentionally change a boundary, update the architecture test in
  the same change and note it.
- A change is not done until a *generated* project builds and tests pass — see the
  `dotnet-template-validation` skill.

## Anti-patterns to reject

- Anemic entities; business logic in handlers/controllers that belongs on the aggregate.
- EF Core / ASP.NET types leaking into Domain or Application contracts.
- Introducing a *generic* repository (`IRepository<T>`) in either tree, or **any** repository /
  Unit of Work in the modular tree (its handlers use `AppDbContext` directly by design; the
  Clean tree's per-aggregate `IEntity1Repository` + `IUnitOfWork` are the existing pattern there).
- Adding MediatR or a second mediator to the Clean Architecture tree; adding *any* mediator
  to the modular tree.
- Cross-module reach-through in `ddvs-modular` instead of `Contracts/`.
- Throwing exceptions for expected business failures instead of `Result`/`Error`.
- Cleverness that makes the generated code hard for users/agents to follow. Prefer the
  boring, copy-a-slice path.
