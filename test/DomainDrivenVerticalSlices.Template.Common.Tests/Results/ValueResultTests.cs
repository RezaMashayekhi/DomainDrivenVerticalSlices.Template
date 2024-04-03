namespace DomainDrivenVerticalSlices.Template.Common.Tests.Results;

using DomainDrivenVerticalSlices.Template.Common.Enums;
using DomainDrivenVerticalSlices.Template.Common.Errors;
using DomainDrivenVerticalSlices.Template.Common.Results;

public class ValueResultTests
{
    private const string ErrorMessage = "error message";
    private const string TestValue = "Test Value Text";
    private readonly IError _error = Error.Create(ErrorType.InvalidInput, ErrorMessage);

    [Fact]
    public void CheckedError_ReturnsErrorMessage_WhenFailureResult()
    {
        // Arrange
        var result = Result<int>.Failure(_error);

        // Act & Assert
        result.CheckedError.Should().Be(_error);
    }

    [Fact]
    public void CheckedError_ThrowsException_WhenValueResultSuccess()
    {
        // Arrange
        var result = Result<string>.Success(TestValue);

        // Act
        Action act = () => { _ = result.CheckedError; };

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Attempted to access CheckedError property when no error is set.");
    }

    [Fact]
    public void Error_ReturnsErrorMessage_WhenFailureResult()
    {
        // Arrange
        var result = Result<string>.Failure(_error);

        // Act & Assert
        result.Error.Should().Be(_error);
    }

    [Fact]
    public void Failure_ReturnsFailureResult()
    {
        // Arrange & Act
        var result = Result<string>.Failure(_error);
        Action act = () => { _ = result.Value; };

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(_error);
        act.Should().Throw<InvalidDataException>()
            .WithMessage("Cannot retrieve value from a failed result.");
    }

    [Fact]
    public void Failure_WithNullError_ThrowsException()
    {
        // Arrange & Act
        Action act = () => Result<string>.Failure(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("*error*");
    }

    [Fact]
    public void IsSuccess_ReturnsFalse_WhenFailureResult()
    {
        // Arrange
        var result = Result<string>.Failure(_error);

        // Act & Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void IsSuccess_ReturnsTrue_WhenSuccessResult()
    {
        // Arrange
        var result = Result<string>.Success(TestValue);

        // Act & Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Success_ReturnsSuccessResultWithValue()
    {
        // Arrange & Act
        var result = Result<string>.Success(TestValue);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Error.Should().BeNull();
        result.Value.Should().Be(TestValue);
    }

    [Fact]
    public void Success_WithNullValue_ThrowsException()
    {
        // Arrange & Act
        Action act = () => Result<string>.Success(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("*value*");
    }

    [Fact]
    public void Success_WithDefaultValue_ThrowsException()
    {
        Action act = () => Result<int>.Success(default);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Value cannot be null or empty for a successful result.*");
    }

    [Fact]
    public void Value_ReturnsValue_WhenSuccessResult()
    {
        // Arrange
        var result = Result<string>.Success(TestValue);

        // Act & Assert
        result.Value.Should().Be(TestValue);
    }

    [Fact]
    public void Value_ThrowsException_WhenFailureResult()
    {
        // Arrange
        var result = Result<string>.Failure(_error);

        // Act
        Action act = () => { _ = result.Value; };

        // Assert
        act.Should().Throw<InvalidDataException>()
            .WithMessage("Cannot retrieve value from a failed result.");
    }
}
