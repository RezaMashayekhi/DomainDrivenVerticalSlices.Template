# Copilot Instructions

This solution is a modular monolith ASP.NET Core API.

## Architecture

- Organize backend code by business module first.
- Inside each module, organize by feature/use case.
- Prefer vertical slices over horizontal technical folders.
- Keep endpoints thin.
- Put business workflow logic in feature handlers.
- Put business invariants in domain objects.
- Put EF Core configuration and external integrations in Infrastructure.
- Use Clean Architecture dependency rules pragmatically.

## Dependency Rules

- Domain code must not depend on ASP.NET Core, EF Core, Infrastructure, cloud SDKs, or messaging libraries.
- Feature handlers may use DbContext directly for simple CRUD features.
- Use explicit interfaces/ports for external systems, complex domain workflows, payment providers, messaging, file storage, and identity providers.
- Do not create generic repositories.
- Do not add abstractions unless they reduce coupling or improve testability.

## Naming

Use predictable names:

- CreateEntity1Request
- CreateEntity1Response
- CreateEntity1Validator
- CreateEntity1Handler
- CreateEntity1Endpoint
- CreateEntity1Tests

## API Rules

- Do not expose EF Core entities directly from endpoints.
- Use explicit request and response DTOs.
- Validate requests before executing business logic.
- Return ProblemDetails for known failures.
- Use Result for expected business errors.

## Testing

- Add or update tests for every behavior change.
- Use unit tests for domain behavior.
- Use integration tests for API + database behavior.
- Prefer Testcontainers for database integration tests.
- Add architecture tests for dependency rules.

## Style

- Keep files small and focused.
- Prefer explicit names.
- Avoid hidden magic.
- Avoid large services with many unrelated methods.
- Avoid unnecessary abstractions.
