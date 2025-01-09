namespace DomainDrivenVerticalSlices.Template.Application.Tests.PipelineBehaviour;

using DomainDrivenVerticalSlices.Template.Application.PipelineBehaviour;
using DomainDrivenVerticalSlices.Template.Common.Enums;
using DomainDrivenVerticalSlices.Template.Common.Results;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;

public class ValidationBehaviourTests
{
    private readonly RequestHandlerDelegate<Result> _next;
    private readonly IRequest<Result> _request;
    private readonly ValidationBehaviour<IRequest<Result>, Result> _validationBehaviour;
    private readonly List<IValidator<IRequest<Result>>> _validators;
    private bool _nextCalled;

    public ValidationBehaviourTests()
    {
        _request = Mock.Of<IRequest<Result>>();
        _validators = [];
        _next = new RequestHandlerDelegate<Result>(() =>
        {
            _nextCalled = true;
            return Task.FromResult(Result.Success());
        });
        _validationBehaviour = new ValidationBehaviour<IRequest<Result>, Result>(_validators);
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenValidatorsIsNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
        {
            new ValidationBehaviour<IRequest<Result>, Result>(null!);
        });

        exception.ParamName.Should().Be("validators");
        exception.Message.Should().Contain("validators");
    }

    [Fact]
    public async Task Handle_WithInvalidRequest_ShouldReturnFailureResultWithValidationError()
    {
        // Arrange
        var expectedErrorMessage = "Error Message";
        var validatorMock = CreateValidatorMock([new("Property", expectedErrorMessage)]);
        _validators.Add(validatorMock.Object);

        // Act
        var result = await _validationBehaviour.Handle(_request, _next, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.CheckedError.ErrorType.Should().Be(ErrorType.InvalidInput);
        result.CheckedError.ErrorMessage.Should().Be(expectedErrorMessage);

        validatorMock.Verify(v => v.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithInvalidRequest_WhenMultipleValidatorsExist_ShouldReturnFailureResultWithMultipleValidationErrors()
    {
        // Arrange
        var validatorMock1 = CreateValidatorMock(
        [
            new("Property1", "Error Message 1"),
            new("Property2", "Error Message 2"),
            new("Property3", "Error Message 3"),
        ]);

        var validatorMock2 = CreateValidatorMock(
        [
            new("Property4", "Error Message 4"),
            new("Property5", "Error Message 5"),
        ]);

        _validators.AddRange(new[]
        {
            validatorMock1.Object,
            validatorMock2.Object,
        });

        // Act
        var result = await _validationBehaviour.Handle(_request, _next, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.InvalidInput, result.CheckedError.ErrorType);

        // Validate that the error messages from all validators are present in the error
        result.CheckedError.ErrorMessage.Should().Contain("Error Message 1");
        result.CheckedError.ErrorMessage.Should().Contain("Error Message 2");
        result.CheckedError.ErrorMessage.Should().Contain("Error Message 3");
        result.CheckedError.ErrorMessage.Should().Contain("Error Message 4");
        result.CheckedError.ErrorMessage.Should().Contain("Error Message 5");

        validatorMock1.Verify(v => v.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>()), Times.Once);
        validatorMock2.Verify(v => v.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithInvalidRequest_WhenNoValidatorsExist_ShouldInvokeNextHandler()
    {
        // Arrange
        _nextCalled = false;

        // Act
        await _validationBehaviour.Handle(_request, _next, CancellationToken.None);

        // Assert
        _nextCalled.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WithInvalidRequest_WhenValidatorsExist_ShouldReturnFailureResultWithValidationError()
    {
        // Arrange
        var validatorMock = CreateValidatorMock([new("Property", "Error Message")]);
        _validators.Add(validatorMock.Object);

        // Act
        var result = await _validationBehaviour.Handle(_request, _next, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.CheckedError.ErrorType.Should().Be(ErrorType.InvalidInput);
        result.CheckedError.ErrorMessage.Should().Be("Error Message");

        validatorMock.Verify(v => v.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNoValidators_ShouldInvokeNextHandler()
    {
        // Arrange
        // No validators added to the list

        // Act
        await _validationBehaviour.Handle(_request, _next, CancellationToken.None);

        // Assert
        _nextCalled.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WithValidatorsExistAndValidationErrors_ShouldNotInvokeNextHandler()
    {
        // Arrange
        var validatorMock = CreateValidatorMock([new("Property", "Error Message")]);
        _validators.Add(validatorMock.Object);

        // Act
        await _validationBehaviour.Handle(_request, _next, CancellationToken.None);

        // Assert
        _nextCalled.Should().BeFalse();

        validatorMock.Verify(v => v.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidatorsExistButNoValidationErrors_ShouldInvokeNextHandler()
    {
        // Arrange
        var validatorMock = CreateValidatorMock([]);
        _validators.Add(validatorMock.Object);

        // Act
        await _validationBehaviour.Handle(_request, _next, CancellationToken.None);

        // Assert
        _nextCalled.Should().BeTrue();

        validatorMock.Verify(v => v.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    private static Mock<IValidator<IRequest<Result>>> CreateValidatorMock(IEnumerable<ValidationFailure> failures)
    {
        var validatorMock = new Mock<IValidator<IRequest<Result>>>();
        validatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(failures));

        return validatorMock;
    }
}
