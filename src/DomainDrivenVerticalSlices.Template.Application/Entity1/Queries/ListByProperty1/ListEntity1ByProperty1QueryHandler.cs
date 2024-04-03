namespace DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.ListByProperty1;

using AutoMapper;
using DomainDrivenVerticalSlices.Template.Application.Dtos;
using DomainDrivenVerticalSlices.Template.Application.Interfaces;
using DomainDrivenVerticalSlices.Template.Common.Results;
using MediatR;

public class ListEntity1ByProperty1QueryHandler(
    IEntity1Repository entity1Repository,
    IMapper mapper) : IRequestHandler<ListEntity1ByProperty1Query, Result<IEnumerable<Entity1Dto>>>
{
    private readonly IEntity1Repository _repository = entity1Repository ?? throw new ArgumentNullException(nameof(entity1Repository));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<Result<IEnumerable<Entity1Dto>>> Handle(ListEntity1ByProperty1Query request, CancellationToken cancellationToken)
    {
        var entities = await _repository.ListAsync(e => e.ValueObject1.Property1.Contains(request.Property1), cancellationToken);
        var dtosList = _mapper.Map<IEnumerable<Entity1Dto>>(entities);
        return Result<IEnumerable<Entity1Dto>>.Success(dtosList);
    }
}
