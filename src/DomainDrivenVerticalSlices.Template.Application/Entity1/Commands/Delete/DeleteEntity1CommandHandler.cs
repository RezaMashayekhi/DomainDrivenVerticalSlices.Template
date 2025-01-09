namespace DomainDrivenVerticalSlices.Template.Application.Entity1.Commands.Delete;

using DomainDrivenVerticalSlices.Template.Application.Interfaces;
using DomainDrivenVerticalSlices.Template.Common.Enums;
using DomainDrivenVerticalSlices.Template.Common.Errors;
using DomainDrivenVerticalSlices.Template.Common.Results;
using MediatR;
using Microsoft.Extensions.Logging;

public class DeleteEntity1CommandHandler(
    IEntity1Repository entity1Repository,
    IUnitOfWork unitOfWork,
    ILogger<DeleteEntity1CommandHandler> logger) : IRequestHandler<DeleteEntity1Command, Result>
{
    private readonly IEntity1Repository _entity1Repository = entity1Repository ?? throw new ArgumentNullException(nameof(entity1Repository));
    private readonly IUnitOfWork unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly ILogger<DeleteEntity1CommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<Result> Handle(DeleteEntity1Command request, CancellationToken cancellationToken)
    {
        var entity1 = await _entity1Repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity1 == null)
        {
            _logger.LogError("Entity1 with id {RequestId} not found.", request.Id);
            return Result.Failure(Error.Create(ErrorType.NotFound, $"Entity1 with id {request.Id} not found."));
        }

        try
        {
            await _entity1Repository.DeleteAsync(entity1, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting Entity1 with id {RequestId}.", request.Id);
            return Result.Failure(Error.Create(ErrorType.OperationFailed, $"Error deleting Entity1 with id {request.Id}."));
        }
    }
}
