# Common Library

The `Common` library provides foundational classes and interfaces that are shared across different layers of the application. This includes error handling mechanisms, models, and results handling, facilitating a consistent approach to common development needs.

## Key Components

### Errors

-   **Error**: A concrete implementation of an error, providing structured error information including types and messages.
-   **IError**: An interface defining the basic structure of an error object.

### Models

-   **ValueObject**: An abstract base class for value objects, implementing equality based on the value rather than the reference.

### Results

-   **IResult**: Interface for a result, indicating success or failure of operations.
-   **Result**: A generic and non-generic implementation for conveying the outcome of operations, optionally carrying a value in case of success.

## Usage

The components in this library are designed to be used throughout the application to ensure a uniform approach to error handling, entity definition, and operation results.

### Creating a New Error

```csharp
var notFoundError = Error.Create(ErrorType.NotFound, "The specified item was not found.");
```

### Handling Operation Results

```csharp
public async Task<Result<Entity1Dto>> Handle(GetEntity1ByIdQuery request, CancellationToken cancellationToken)
{
    var entity1 = await _entity1Repository.GetByIdAsync(request.Id, cancellationToken);
    if (entity1 == null)
    {
        return Result<Entity1Dto>.Failure(Error.Create(ErrorType.NotFound, $"Entity1 with id {request.Id} not found."));
    }

    var entity1Dto = entity1.MapToDto();
    return Result<Entity1Dto>.Success(entity1Dto);
}
```

### Defining a Value Object

```csharp
public class EmailAddress : ValueObject<EmailAddress>
{
    public string Value { get; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    // Additional implementation details...
}
```
