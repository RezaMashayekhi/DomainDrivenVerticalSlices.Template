namespace DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.ListByProperty1;

using AutoMapper;
using DomainDrivenVerticalSlices.Template.Application.Dtos;
using DomainDrivenVerticalSlices.Template.Application.Interfaces;
using DomainDrivenVerticalSlices.Template.Common.Enums;
using DomainDrivenVerticalSlices.Template.Common.Errors;
using DomainDrivenVerticalSlices.Template.Common.Results;
using MediatR;
using Microsoft.Extensions.Logging;

public class ListEntity1ByProperty1QueryHandler(
    IEntity1Repository entity1Repository,
    ILogger<ListEntity1ByProperty1QueryHandler> logger,
    IMapper mapper) : IRequestHandler<ListEntity1ByProperty1Query, Result<IEnumerable<Entity1Dto>>>
{
    private readonly IEntity1Repository _repository = entity1Repository ?? throw new ArgumentNullException(nameof(entity1Repository));
    private readonly ILogger<ListEntity1ByProperty1QueryHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<Result<IEnumerable<Entity1Dto>>> Handle(ListEntity1ByProperty1Query request, CancellationToken cancellationToken)
    {
        try
        {
            var entities = await _repository.ListAsync(e => e.ValueObject1.Property1.Contains(request.Property1), cancellationToken);
            var dtosList = _mapper.Map<IEnumerable<Entity1Dto>>(entities);
            return Result<IEnumerable<Entity1Dto>>.Success(dtosList);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error listing Entity1 by Property1.");
            return Result<IEnumerable<Entity1Dto>>.Failure(Error.Create(ErrorType.OperationFailed, "Error listing Entity1 by Property1."));
        }
    }
}
