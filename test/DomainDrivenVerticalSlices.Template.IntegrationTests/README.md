# Integration Tests

The Integration Tests project for `DomainDrivenVerticalSlices.Template` aims to validate the application's behavior as a whole, ensuring that all layers (Web API, Application, Domain, and Infrastructure) work together seamlessly. These tests simulate real-world scenarios as closely as possible, providing confidence in the application's functionality and stability.

## Key Components

### Helpers
- **ErrorDto**: Represents the structure of error responses for verifying error handling in integration scenarios.
- **ListLogger** and **ListLoggerProvider**: Utilities for capturing log output during test execution, enabling assertions on logging behavior.

### CustomWebApplicationFactory
A custom factory for bootstrapping the application in a test environment, allowing for tests to run against an in-memory database and simplified service configuration.

### Entity1Tests
Examples of comprehensive tests for `Entity1`, covering common CRUD operations and ensuring correct responses and system states.

## Setup and Execution

1. Ensure local settings, such as connection strings in `appsettings.integrationtest.json`, are correctly configured for the test environment.
2. Use the `CustomWebApplicationFactory` to create instances of the test server and client as needed for your tests.
3. Run the tests using the .NET CLI with `dotnet test` or through your preferred IDE's test runner.

## Writing New Tests

When adding new features or modifying existing functionalities, accompany your changes with integration tests that:

1. Reflect the real-world usage of your APIs.
2. Cover both success and error pathways.
3. Utilize the `ListLogger` to assert important log messages and error handling.
