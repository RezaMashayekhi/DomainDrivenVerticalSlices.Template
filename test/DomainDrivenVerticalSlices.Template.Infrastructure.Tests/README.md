# Infrastructure Tests

This project contains tests for the Infrastructure layer, ensuring that the data access and other infrastructure-related functionalities work as expected.

## Test Structure

Tests are organized according to the components they are testing:

### Data
- **Entity1RepositoryTests**: Tests the repository operations for `Entity1`, ensuring that data access works as expected.
- **UnitOfWorkTests**: Verifies that the Unit of Work correctly coordinates the repositories.

## Running Tests

To run the infrastructure tests, execute the following command in the `Infrastructure.Tests` directory:

```bash
dotnet test
```