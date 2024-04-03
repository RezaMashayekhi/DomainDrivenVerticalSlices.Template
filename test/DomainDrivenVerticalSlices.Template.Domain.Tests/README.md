# Domain Tests

This project contains unit tests for the Domain layer, validating that the domain entities, value objects, and events behave as expected according to the business rules and logic defined.

## Test Structure

Tests are organized to mirror the Domain layer's structure:

### Entities
Tests within `Entities` ensure that entity behaviors and business rules are correctly implemented. For example, `Entity1Tests` verifies the behavior of `Entity1`.

### Value Objects
Tests in `ValueObjects` validate the immutability, equality, and other characteristics of value objects, such as `ValueObject1Tests` for `ValueObject1`.

## Running Tests

Execute the following command in the `Domain.Tests` directory to run all tests:

```bash
dotnet test
```