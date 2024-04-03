# Web API Unit Tests

This project is dedicated to unit testing the `WebApi` layer, ensuring that each controller operates as expected in isolation from the rest of the application. By focusing on unit tests, we aim to verify the logic within our controllers, including routing, request handling, and response formatting.

## Structure

### Controllers
- **Entity1ControllerTests.cs**: Tests the operations provided by `Entity1Controller`, covering scenarios for each action method. These tests mock out dependencies to focus solely on controller behavior.

## Running Tests

To run the WebApi tests, execute the following command in the `WebApi.Tests` directory:

```bash
dotnet test
```