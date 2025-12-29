# Web API

The `WebApi` project serves as the entry point to the application, exposing the core functionalities of the domain through HTTP endpoints. It's built on ASP.NET Core and follows RESTful principles to provide a clear and intuitive interface for client applications.

## Key Components

### Configurations

- **ConfigurationsManager.cs**: Manages application configurations, facilitating the setup of different environments like development, production, etc.

### Controllers

- **Entity1Controller.cs**: Demonstrates how to implement an API controller with CRUD operations for `Entity1`.

### Extensions

- **AppExtensions.cs**: Contains middleware configurations for the application.
- **LoggerExtensions.cs**: Configures logging throughout the application lifecycle.
- **ServiceExtensions.cs**: Centralizes service registration and DI container setup.

## Setup and Running

1. Ensure you have the .NET SDK and runtime installed.
2. Navigate to the project directory and run `dotnet restore` to install dependencies.
3. Use `dotnet run` to start the application. By default, it listens on `https://localhost:5246` and `http://localhost:5245`.

## Extending the Web API

To add new endpoints or modify existing ones:

1. Create new controllers or update existing ones in the `Controllers` directory.
2. Utilize the custom mediator for handling business logic, ensuring a clean separation of concerns.
3. Register any new services or dependencies in `ServiceExtensions.cs`.

For detailed configuration changes, refer to `ConfigurationsManager.cs` and environment-specific `appsettings` files.
