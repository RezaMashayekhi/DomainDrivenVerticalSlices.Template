namespace DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.GetById;

using DomainDrivenVerticalSlices.Template.Application.Dtos;
using DomainDrivenVerticalSlices.Template.Application.Interfaces;
using DomainDrivenVerticalSlices.Template.Application.Mappings;
using DomainDrivenVerticalSlices.Template.Common.Enums;
using DomainDrivenVerticalSlices.Template.Common.Errors;
using DomainDrivenVerticalSlices.Template.Common.Results;
using MediatR;
using Microsoft.Extensions.Logging;

public class GetEntity1ByIdQueryHandler(
    IEntity1Repository entity1Repository,
    ILogger<GetEntity1ByIdQueryHandler> logger) : IRequestHandler<GetEntity1ByIdQuery, Result<Entity1Dto>>
{
    private readonly IEntity1Repository _entity1Repository = entity1Repository ?? throw new ArgumentNullException(nameof(entity1Repository));
    private readonly ILogger<GetEntity1ByIdQueryHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<Result<Entity1Dto>> Handle(GetEntity1ByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var entity1 = await _entity1Repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity1 == null)
            {
                _logger.LogError("Entity1 with id {RequestId} not found.", request.Id);
                return Result<Entity1Dto>.Failure(Error.Create(ErrorType.NotFound, $"Entity1 with id {request.Id} not found."));
            }

            var entity1Dto = entity1.MapToDto();
            return Result<Entity1Dto>.Success(entity1Dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving Entity1 by id.");
            return Result<Entity1Dto>.Failure(Error.Create(ErrorType.OperationFailed, ex.Message));
        }
    }
}
