namespace DomainDrivenVerticalSlices.Template.Application.Entity1.Commands.Update;

using DomainDrivenVerticalSlices.Template.Application.Interfaces;
using DomainDrivenVerticalSlices.Template.Common.Enums;
using DomainDrivenVerticalSlices.Template.Common.Errors;
using DomainDrivenVerticalSlices.Template.Common.Results;
using DomainDrivenVerticalSlices.Template.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

public class UpdateEntity1CommandHandler(
    IEntity1Repository entity1Repository,
    IUnitOfWork unitOfWork,
    ILogger<UpdateEntity1CommandHandler> logger) : IRequestHandler<UpdateEntity1Command, Result>
{
    private readonly IEntity1Repository _entity1Repository = entity1Repository ?? throw new ArgumentNullException(nameof(entity1Repository));
    private readonly IUnitOfWork unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly ILogger<UpdateEntity1CommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<Result> Handle(UpdateEntity1Command request, CancellationToken cancellationToken)
    {
        var entity1 = await _entity1Repository.GetByIdAsync(request.Entity1.Id, cancellationToken);
        if (entity1 == null)
        {
            _logger.LogError("Entity1 with id {Entity1Id} not found.", request.Entity1.Id);
            return Result.Failure(Error.Create(ErrorType.NotFound, $"Entity1 with id {request.Entity1.Id} not found."));
        }

        var valueObject1Result = ValueObject1.Create(request.Entity1.ValueObject1.Property1);
        if (!valueObject1Result.IsSuccess)
        {
            _logger.LogError("An error occurred: {ErrorMessage}", valueObject1Result.CheckedError.ErrorMessage);
            return Result.Failure(valueObject1Result.CheckedError);
        }

        entity1.Update(valueObject1Result.Value);

        try
        {
            await _entity1Repository.UpdateAsync(entity1, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating Entity1 with id {Entity1Id}.", request.Entity1.Id);
            return Result.Failure(Error.Create(ErrorType.OperationFailed, $"Error updating Entity1 with id {request.Entity1.Id}."));
        }
    }
}
