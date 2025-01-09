namespace DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.GetAll;

using AutoMapper;
using DomainDrivenVerticalSlices.Template.Application.Dtos;
using DomainDrivenVerticalSlices.Template.Application.Interfaces;
using DomainDrivenVerticalSlices.Template.Common.Enums;
using DomainDrivenVerticalSlices.Template.Common.Errors;
using DomainDrivenVerticalSlices.Template.Common.Results;
using MediatR;
using Microsoft.Extensions.Logging;

public class GetEntity1AllQueryHandler(
    IEntity1Repository entity1Repository,
    ILogger<GetEntity1AllQueryHandler> logger,
    IMapper mapper) : IRequestHandler<GetEntity1AllQuery, Result<IEnumerable<Entity1Dto>>>
{
    private readonly IEntity1Repository _entity1Repository = entity1Repository ?? throw new ArgumentNullException(nameof(entity1Repository));
    private readonly ILogger<GetEntity1AllQueryHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<Result<IEnumerable<Entity1Dto>>> Handle(GetEntity1AllQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var entities = await _entity1Repository.GetAllAsync(cancellationToken);

            var dtos = _mapper.Map<IEnumerable<Entity1Dto>>(entities ?? []);
            return Result<IEnumerable<Entity1Dto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all Entity1.");
            return Result<IEnumerable<Entity1Dto>>.Failure(Error.Create(ErrorType.OperationFailed, "Error retrieving all Entity1."));
        }
    }
}
