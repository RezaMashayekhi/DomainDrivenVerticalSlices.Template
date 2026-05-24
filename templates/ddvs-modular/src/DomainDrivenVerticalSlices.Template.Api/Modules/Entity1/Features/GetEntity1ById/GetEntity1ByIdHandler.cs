namespace DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Features.GetEntity1ById;

using DomainDrivenVerticalSlices.Template.Api.Common.Errors;
using DomainDrivenVerticalSlices.Template.Api.Common.Persistence;
using DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Contracts;
using DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Domain;
using Microsoft.EntityFrameworkCore;

internal sealed class GetEntity1ByIdHandler(AppDbContext dbContext)
{
    public async Task<Result<GetEntity1ByIdResponse>> HandleAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        Entity1Dto? entity1 = await dbContext.Entities1
            .Where(entity1 => entity1.Id == id)
            .Select(entity1 => new Entity1Dto(
                entity1.Id,
                new ValueObject1Dto(entity1.ValueObject1.Property1)))
            .FirstOrDefaultAsync(cancellationToken);

        if (entity1 is null)
        {
            return Result<GetEntity1ByIdResponse>.Failure(Entity1Errors.NotFound(id));
        }

        return new GetEntity1ByIdResponse(entity1);
    }
}
