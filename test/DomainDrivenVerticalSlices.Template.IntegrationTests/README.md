# Integration Tests

The Integration Tests project validates the application's behavior as a whole, ensuring that all layers (Web API, Application, Domain, and Infrastructure) work together seamlessly. These tests simulate real-world scenarios as closely as possible.

## Test Factories

### CustomWebApplicationFactory

A custom factory for bootstrapping the application in a test environment with an **in-memory SQLite database**. Ideal for fast, isolated tests.

### TestcontainersWebApplicationFactory

An alternative factory using **Testcontainers** to run tests against a real SQLite database in a Docker container. Provides more realistic testing conditions:

```csharp
public class Entity1TestsWithContainers : IClassFixture<TestcontainersWebApplicationFactory>
{
    private readonly HttpClient _client;

    public Entity1TestsWithContainers(TestcontainersWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }
}
```

**Prerequisites**: Docker must be running for Testcontainers tests.

## Key Components

### Helpers

- **ErrorDto**: Represents the structure of error responses for verifying error handling.
- **ListLogger** and **ListLoggerProvider**: Utilities for capturing log output during test execution.

### Entity1Tests

Comprehensive tests for `Entity1`, covering common CRUD operations and ensuring correct responses and system states.

## Setup and Execution

1. Configure connection strings in `appsettings.integrationtest.json` if needed.
2. For Testcontainers tests, ensure Docker is running.
3. Run tests using:

```bash
dotnet test
```

## Choosing a Test Factory

| Factory                               | Database                 | Speed  | Realism | Docker Required |
| ------------------------------------- | ------------------------ | ------ | ------- | --------------- |
| `CustomWebApplicationFactory`         | In-memory SQLite         | Fast   | Good    | No              |
| `TestcontainersWebApplicationFactory` | Real SQLite in container | Slower | High    | Yes             |

Use `CustomWebApplicationFactory` for CI pipelines where speed matters, and `TestcontainersWebApplicationFactory` when you need to test database-specific behavior.
