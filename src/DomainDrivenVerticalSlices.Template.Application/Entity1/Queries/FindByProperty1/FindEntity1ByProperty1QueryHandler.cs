namespace DomainDrivenVerticalSlices.Template.Application.Entity1.Queries.FindByProperty1;

using AutoMapper;
using DomainDrivenVerticalSlices.Template.Application.Dtos;
using DomainDrivenVerticalSlices.Template.Application.Interfaces;
using DomainDrivenVerticalSlices.Template.Common.Enums;
using DomainDrivenVerticalSlices.Template.Common.Errors;
using DomainDrivenVerticalSlices.Template.Common.Results;
using MediatR;

public class FindEntity1ByProperty1QueryHandler(
    IEntity1Repository entity1Repository,
    IMapper mapper) : IRequestHandler<FindEntity1ByProperty1Query, Result<Entity1Dto>>
{
    private readonly IEntity1Repository _entity1Repository = entity1Repository ?? throw new ArgumentNullException(nameof(entity1Repository));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<Result<Entity1Dto>> Handle(FindEntity1ByProperty1Query request, CancellationToken cancellationToken)
    {
        var entity = await _entity1Repository.FindAsync(e => e.ValueObject1.Property1 == request.Property1, cancellationToken);
        if (entity == null)
        {
            return Result<Entity1Dto>.Failure(Error.Create(ErrorType.NotFound, "Entity not found."));
        }

        var dto = _mapper.Map<Entity1Dto>(entity);
        return Result<Entity1Dto>.Success(dto);
    }
}
