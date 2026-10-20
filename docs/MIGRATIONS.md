# Migration notes

This document records changes that affect code in generated projects. Version numbers refer to
the NuGet template package.

## 10.8.0

### `Result<TValue>` default-value behavior

`Result<TValue>.Success` now accepts valid default values such as `0`, `false`, and a default
struct. The `Value` getter throws `InvalidOperationException` when called on a failed result
or when no success value exists. Code that depended on rejecting default value types or caught
`InvalidDataException` for an unavailable result value should be updated to the new behavior.

### SQLite integration test host

The generated Clean Architecture integration tests now use an in-host SQLite database through
`SqliteWebApplicationFactory`; the unused Alpine/Testcontainers container and package
references have been removed. Projects that customized `TestcontainersWebApplicationFactory`
should migrate their setup to the SQLite factory, or add a real database provider and container
configuration for their own environment.

### Modular persistence and uniqueness conflicts

The modular API owns `AppDbContext` registration at its common persistence boundary. The
Entity1 create/update handlers translate SQLite uniqueness races into HTTP 409. If you copied
these handlers into a project with another database provider, map that provider's unique
constraint error codes in the provider-specific error mapper.

### React UI dependency updates

New React UI projects use React Router 7.18.4 to resolve npm security advisories. The template
uses declarative routes, links, and navigation hooks that continue to work with this release;
existing projects are not changed automatically. If upgrading an existing UI, review the
React Router v7 migration guidance and run `npm ci`, `npm run build`, and `npm test -- --run`.
The supported Node.js lines are 20.19+, 22.13+, and 24+ for the updated Vite/SWC toolchain.

## Earlier versions

See [CHANGELOG.md](../CHANGELOG.md) for behavior changes through 10.7.0. Changes listed there
predate this migration guide and retain their original release context.
