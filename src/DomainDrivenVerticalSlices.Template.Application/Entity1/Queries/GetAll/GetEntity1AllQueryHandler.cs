namespace DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.GetAll;

using AutoMapper;
using DomainDrivenVerticalSlices.Template.Application.Dtos;
using DomainDrivenVerticalSlices.Template.Application.Interfaces;
using DomainDrivenVerticalSlices.Template.Common.Results;
using DomainDrivenVerticalSlices.Template.Domain.Entities;
using MediatR;

public class GetEntity1AllQueryHandler(
    IEntity1Repository entity1Repository,
    IMapper mapper) : IRequestHandler<GetEntity1AllQuery, Result<IEnumerable<Entity1Dto>>>
{
    private readonly IEntity1Repository _entity1Repository = entity1Repository ?? throw new ArgumentNullException(nameof(entity1Repository));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<Result<IEnumerable<Entity1Dto>>> Handle(GetEntity1AllQuery request, CancellationToken cancellationToken)
    {
        var entities = await _entity1Repository.GetAllAsync(cancellationToken);

        var dtos = _mapper.Map<IEnumerable<Entity1Dto>>(entities ?? new List<Entity1>());
        return Result<IEnumerable<Entity1Dto>>.Success(dtos);
    }
}
