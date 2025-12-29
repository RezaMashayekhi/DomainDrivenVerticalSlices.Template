namespace DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.GetAll;

using DomainDrivenVerticalSlices.Template.Application.Dtos;
using DomainDrivenVerticalSlices.Template.Application.Interfaces;
using DomainDrivenVerticalSlices.Template.Application.Mappings;
using DomainDrivenVerticalSlices.Template.Common.Enums;
using DomainDrivenVerticalSlices.Template.Common.Errors;
using DomainDrivenVerticalSlices.Template.Common.Mediator;
using DomainDrivenVerticalSlices.Template.Common.Results;
using Microsoft.Extensions.Logging;

public class GetEntity1AllQueryHandler(
    IEntity1Repository entity1Repository,
    ILogger<GetEntity1AllQueryHandler> logger) : IQueryHandler<GetEntity1AllQuery, Result<IEnumerable<Entity1Dto>>>
{
    private readonly IEntity1Repository _entity1Repository = entity1Repository ?? throw new ArgumentNullException(nameof(entity1Repository));
    private readonly ILogger<GetEntity1AllQueryHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<Result<IEnumerable<Entity1Dto>>> Handle(GetEntity1AllQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var entities = await _entity1Repository.GetAllAsync(cancellationToken);

            var dtos = (entities ?? []).MapToDto();
            return Result<IEnumerable<Entity1Dto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all Entity1.");
            return Result<IEnumerable<Entity1Dto>>.Failure(Error.Create(ErrorType.OperationFailed, "Error retrieving all Entity1."));
        }
    }
}
