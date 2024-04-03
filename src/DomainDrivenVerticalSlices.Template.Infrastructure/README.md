# Infrastructure Layer

The Infrastructure layer provides technical capabilities to support the application's operations, such as data access mechanisms, external services integration, and other cross-cutting concerns. This layer is implemented with the principle of inversion of control in mind, ensuring that the domain layer remains agnostic of infrastructure concerns.

## Key Components

### Data
- **AppDbContext**: The Entity Framework Core context, acting as the primary class that coordinates Entity Framework functionality for the application's data model.
- **Entity1Repository**: An implementation of the repository pattern for `Entity1`, encapsulating all data access for the entity.
- **UnitOfWork**: Implements the Unit of Work pattern, coordinating the work of multiple repositories by creating a single database context instance shared across them.

### Extensions
- **InfrastructureExtensions.cs**: Contains extension methods for registering the infrastructure services in the application's dependency injection container.

### Migrations
Includes Entity Framework Core migrations for evolving the database schema over time.

## Configuring the Infrastructure Layer

To integrate the Infrastructure layer into your application, use the `InfrastructureExtensions` class:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddInfrastructure(Configuration);
}
```

