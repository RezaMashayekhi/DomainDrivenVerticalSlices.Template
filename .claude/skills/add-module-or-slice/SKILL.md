---
name: add-module-or-slice
description: Use when adding a new feature slice to the Clean Architecture template (src/*.Application) or a new business module / feature slice to the modular template (templates/ddvs-modular). Gives the full touchpoint checklist so nothing (registration, EF config, tests, docs) is missed.
---

# Add a Module or Slice

Two trees, two recipes. In both: copy the closest existing sibling, rename consistently, and
finish with the `dotnet-template-validation` smoke of the variant you touched.

## A. Clean Architecture tree — new feature slice (`ddvs` / `ddvs-clean`)

A slice lives in `src/DomainDrivenVerticalSlices.Template.Application/<Entity>/{Commands|Queries|Events}/<Feature>/`
with `<Feature><Entity>Command|Query` + `...Handler` + `...Validator` (see
`Entity1/Commands/Create/` as the reference).

1. **Domain**: entity/value-object changes in `*.Domain` (factory methods return `Result<T>`;
   raise events with `AddDomainEvent(...)` — `DispatchDomainEventsInterceptor` publishes them
   automatically **after** a successful save; do **not** also publish manually from the
   handler, or the event fires twice).
2. **Application**: command/query record + handler + FluentValidation validator in one slice
   folder. **No registration edits needed** — `ApplicationExtensions.AddApplication` scans the
   assembly for `ICommandHandler`/`IQueryHandler`/`INotificationHandler` implementations and
   registers validators via `AddValidatorsFromAssembly`.
3. **Infrastructure**: repository/interface additions if the slice needs new data access
   (`IEntity1Repository` pattern), EF configuration under `*.Infrastructure/Data`.
4. **WebApi — both API styles**: add the action to `Controllers/**` **and** the route to
   `Endpoints/**`. `--ApiType` picks one at generation time; they must expose the same
   routes, status codes, and error shapes.
5. **Tests**: validator tests + handler tests in `*.Application.Tests/<Entity>/...`,
   controller **and** endpoint tests in `*.WebApi.Tests`, and an integration test in
   `*.IntegrationTests` if the slice adds a route.
6. **Docs**: the per-project READMEs (`src/*/README.md`) describe patterns, not features —
   update only if you changed a pattern.
7. **Validate**: smoke both `--ApiType Controller` and `--ApiType MinimalApi` (the slice
   touches both) per `dotnet-template-validation`.

## B. Modular tree — new module (`ddvs-modular`)

A module is `Modules/<Module>/{Contracts,Domain,Endpoints,Features,Infrastructure}` +
`<Module>Module.cs` (see `Modules/Entity1/` in
`templates/ddvs-modular/src/DomainDrivenVerticalSlices.Template.Api/`).

1. Copy the `Entity1` module layout. Keep types `internal` where the sample does.
2. **`<Module>Module.cs`**: an `internal static class` with
   `Add<Module>Module(this IServiceCollection, IConfiguration)` that registers each feature
   handler (`services.AddScoped<CreateXHandler>()` — handlers are **explicitly** registered,
   not scanned) and validators via `AddValidatorsFromAssemblyContaining<...>(includeInternalTypes: true)`.
   Shared persistence is registered once through `AddPersistence` in Program.cs.
   Additional modules register only their own handlers and validators.
3. **Program.cs**: add `builder.Services.Add<Module>Module();`.
4. **Persistence**: EF entity configuration stays inside `Modules/<Module>/Infrastructure/`;
   expose the `DbSet` through `Common/Persistence/AppDbContext`.
5. **Cross-module communication** goes through `Contracts/` only (integration events, DTOs) —
   never reference another module's `Infrastructure`, `Domain`, or `Features` internals.
   Note the events caveat: domain/integration events are **inert extension points** here —
   there is deliberately no mediator or event pipeline (see the Entity1 module README).
6. **Flow per feature slice**: `Features/<Slice>/` holds `Request`/`Handler`/`Validator`/
   `Response`; the endpoint (in `Endpoints/`) calls the validator, then the handler, then maps
   `Result`/`Error` via `Common/Errors` + `Common/Endpoints` to ProblemDetails. No mediator,
   no generic repository, no Unit of Work.
7. **Tests**: unit tests (`tests/*.UnitTests`), integration tests per endpoint
   (`tests/*.IntegrationTests`), and extend `tests/*.ArchitectureTests` so the new module is
   covered by the dependency rules (the cross-module test is vacuous with a single module).
8. **Docs**: add a module README (mirror `Modules/Entity1/README.md`) and update
   `docs/MAINTAINER_GUIDE.md` if the repository's module-structure rules change.
9. **Validate**: smoke `ddvs-modular` with `--UseAspire true` **and** `false`.
