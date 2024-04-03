namespace DomainDrivenVerticalSlices.Template.Common.Tests.Results;

using DomainDrivenVerticalSlices.Template.Common.Enums;
using DomainDrivenVerticalSlices.Template.Common.Errors;
using DomainDrivenVerticalSlices.Template.Common.Results;

public class ResultTests
{
    private const string ErrorMessage = "error message";
    private readonly IError _error = Error.Create(ErrorType.InvalidInput, ErrorMessage);

    [Fact]
    public void CheckedError_ReturnsError_WhenFailureResult()
    {
        // Arrange
        var result = Result.Failure(_error);

        // Act & Assert
        result.CheckedError.Should().Be(_error);
    }

    [Fact]
    public void CheckedError_ThrowsException_WhenSuccessResult()
    {
        // Arrange
        var result = Result.Success();

        // Act
        Action act = () => { _ = result.CheckedError; };

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Attempted to access CheckedError property when no error is set.");
    }

    [Fact]
    public void Error_ReturnsError_WhenFailureResult()
    {
        // Arrange
        var result = Result.Failure(_error);

        // Act & Assert
        result.Error.Should().Be(_error);
    }

    [Fact]
    public void Failure_ReturnsFailureResult()
    {
        // Arrange & Act
        var result = Result.Failure(_error);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(_error);
    }

    [Fact]
    public void Failure_ThrowsException_WhenErrorIsNull()
    {
        // Arrange & Act
        Action act = () => { _ = Result.Failure(null!); };

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("*error*");
    }

    [Fact]
    public void IsSuccess_ReturnsFalse_WhenFailureResult()
    {
        // Arrange
        var result = Result.Failure(_error);

        // Act & Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void IsSuccess_ReturnsTrue_WhenSuccessResult()
    {
        // Arrange
        var result = Result.Success();

        // Act & Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Success_ReturnsSuccessResult()
    {
        // Arrange & Act
        var result = Result.Success();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Error.Should().BeNull();
    }
}
