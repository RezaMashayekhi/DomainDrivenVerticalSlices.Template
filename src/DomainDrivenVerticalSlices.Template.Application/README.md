# Application Layer

The Application layer serves as the core of our business logic, orchestrating the application's behavior through commands, queries, and events. It's structured to support a clean architecture, ensuring that our domain logic is separate from infrastructure concerns.

## Key Components

- **Commands and Queries**: Define the operations that can be performed in the application, such as creating, updating, and retrieving entities.
- **Events**: Handle application-wide events, facilitating decoupled components interaction.
- **DTOs (Data Transfer Objects)**: Serve as the data structure for transferring data between the application layer and other layers.
- **Interfaces**: Define contracts for repository interactions and user context access (`IUser`), ensuring abstraction over concrete implementations.
- **Mappings**: Manual mapping extensions between domain entities and DTOs for optimal performance and explicit control.
- **Pipeline Behaviors**: Interceptors that add cross-cutting concerns to mediator request handling.

## Pipeline Behaviors

### LoggingBehaviour

Automatically logs the execution of commands and queries, wrapping the handling process and logging key details about the request and its result. Sensitive information is intentionally omitted.

### ValidationBehaviour

Ensures that incoming requests comply with business rules by integrating with FluentValidation for automatic validation of commands and queries.

### PerformanceBehaviour

Monitors request execution time and logs warnings when requests exceed a configurable threshold (default: 500ms). Helps identify slow-running operations that may need optimization.

### UnhandledExceptionBehaviour

Centralized exception handling in the mediator pipeline. Logs detailed exception information for debugging while preventing exceptions from silently failing.

## Interfaces

### IUser

Abstraction for accessing the current user context:

```csharp
public interface IUser
{
    string? Id { get; }
}
```

Used by `BaseAuditableEntity` interceptors to populate `CreatedBy` and `ModifiedBy` fields automatically.

## Setting Up the Application Layer

The `ApplicationExtensions` class simplifies the setup:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddApplication();
}
```

This registers the custom mediator, all handlers, validators, and pipeline behaviors.
