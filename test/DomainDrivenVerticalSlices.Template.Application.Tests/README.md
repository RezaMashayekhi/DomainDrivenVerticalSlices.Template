# Application Tests

This directory contains the tests for the application layer. Our tests are organized to reflect the structure of the application itself, ensuring that each component, from commands and queries to event handlers, is thoroughly tested.

## Structure

The tests are organized into several categories, mirroring the application's structure:

-   **Commands**: Tests for command handlers and validators, ensuring that the logic for creating, updating, and deleting entities works as expected.
-   **Queries**: Tests for query handlers and validators, verifying that data retrieval operations perform correctly.
-   **Events**: Tests for event handlers, confirming that application events trigger the intended side effects.
-   **PipelineBehaviour**: Tests for mediator pipeline behaviors, such as logging and validation, to ensure they are applied correctly across the application.
-   **Helpers**: Includes mocks and stubs to support the testing of components.

## Running Tests

To run the tests, navigate to the `Application.Tests` directory and execute the following command:

```bash
dotnet test
```

## Utilizing MockExtensions for Logging Assertions

To simplify the process of verifying logging occurs, we've introduced `MockExtensions`, a set of extension methods for `Mock<ILogger<T>>` that streamline the assertions of log messages.

### Features of MockExtensions

-   **VerifyLogging**: Allows you to assert that a specific log message was written at a designated log level. This method is particularly useful for confirming that important information is logged correctly and at the appropriate times.

-   **VerifyLogLevelTotalCalls**: Helps in asserting that log messages at a specific log level were written a certain number of times. This is useful for ensuring that repetitive actions in the application result in the expected number of log entries.

-   **VerifyNotLogged**: Ensures that a specific message was not logged at a specified log level, useful for making sure certain logs should not appear.

### Example Usage

Here's a simple example of how `MockExtensions` can be used in a test to verify that a log message was written once at the `Information` level:

```csharp
var loggerMock = new Mock<ILogger<MyComponent>>();
var myComponent = new MyComponent(loggerMock.Object);

myComponent.PerformAction();

loggerMock.VerifyLogging("Action performed successfully", LogLevel.Information);
```
