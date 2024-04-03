namespace DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.GetById;

using AutoMapper;
using DomainDrivenVerticalSlices.Template.Application.Dtos;
using DomainDrivenVerticalSlices.Template.Application.Interfaces;
using DomainDrivenVerticalSlices.Template.Common.Enums;
using DomainDrivenVerticalSlices.Template.Common.Errors;
using DomainDrivenVerticalSlices.Template.Common.Results;
using MediatR;
using Microsoft.Extensions.Logging;

public class GetEntity1ByIdQueryHandler(
    IEntity1Repository entity1Repository,
    ILogger<GetEntity1ByIdQueryHandler> logger,
    IMapper mapper) : IRequestHandler<GetEntity1ByIdQuery, Result<Entity1Dto>>
{
    private readonly IEntity1Repository _entity1Repository = entity1Repository ?? throw new ArgumentNullException(nameof(entity1Repository));
    private readonly ILogger<GetEntity1ByIdQueryHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<Result<Entity1Dto>> Handle(GetEntity1ByIdQuery request, CancellationToken cancellationToken)
    {
        var entity1 = await _entity1Repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity1 == null)
        {
            _logger.LogError("Entity1 with id {requestId} not found.", request.Id);
            return Result<Entity1Dto>.Failure(Error.Create(ErrorType.NotFound, $"Entity1 with id {request.Id} not found."));
        }

        var entity1Dto = _mapper.Map<Entity1Dto>(entity1);
        return Result<Entity1Dto>.Success(entity1Dto);
    }
}
