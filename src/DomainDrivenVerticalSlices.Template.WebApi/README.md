# Web API

The `WebApi` project serves as the entry point to the application, exposing the core functionalities of the domain through HTTP endpoints. It's built on ASP.NET Core and provides either traditional controllers or modern minimal API endpoints, depending on the `--ApiType` template parameter.

## Key Components

### Controllers (when `--ApiType Controller`)

- **Entity1Controller.cs**: Traditional MVC-style API controller with CRUD operations for `Entity1`.

### Minimal API Endpoints (when `--ApiType MinimalApi`)

- **Entity1Endpoints.cs**: Modern minimal API implementation using `EndpointGroupBase` pattern — a cleaner alternative to controllers for simple CRUD operations.
- **EndpointGroupBase**: Abstract base class for organizing minimal API endpoint groups.
- **EndpointRouteBuilderExtensions**: Infrastructure for auto-discovering and mapping endpoint groups.

### Infrastructure

- **CurrentUser.cs**: Implementation of `IUser` interface, providing access to the current user's identity for audit fields.
- **WebApplicationExtensions.cs**: Extensions for mapping minimal API endpoint groups (MinimalApi only).

### Extensions

- **AppExtensions.cs**: Middleware configurations including CORS, HTTPS redirection, and endpoint mapping.
- **LoggerExtensions.cs**: Configures logging throughout the application lifecycle.
- **ServiceExtensions.cs**: Centralizes service registration, including `CurrentUser` for the `IUser` abstraction.

## Controllers vs Minimal APIs

Choose your API style when creating the project:

```bash
dotnet new ddvs -n MyProject                      # Controller (default)
dotnet new ddvs -n MyProject --ApiType MinimalApi  # Minimal API
```

| Aspect       | Controllers                  | Minimal APIs               |
| ------------ | ---------------------------- | -------------------------- |
| Best for     | Complex APIs, existing teams | Simple CRUD, microservices |
| Features     | Full MVC pipeline, filters   | Lightweight, fast startup  |
| Organization | By resource/controller       | By endpoint group          |

Both approaches use the same mediator pattern for business logic and route to `/api/{Entity}`.

## Setup and Running

1. Ensure you have the .NET SDK and runtime installed.
2. Navigate to the project directory and run `dotnet restore`.
3. Use `dotnet run` to start the application. Default ports: `https://localhost:7194` and `http://localhost:5246`.

Or use .NET Aspire from the `AppHost` project for orchestrated startup.

## Extending the Web API

To add new endpoints:

**Using Controllers:**

1. Create a new controller in the `Controllers` directory.
2. Inject `IMediator` and dispatch commands/queries.

**Using Minimal APIs:**

1. Create a new class inheriting from `EndpointGroupBase`.
2. Override `Map()` to define routes.
3. Endpoints are auto-discovered and registered.
