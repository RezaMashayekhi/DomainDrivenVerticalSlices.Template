namespace DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Features.CreateEntity1;

using DomainDrivenVerticalSlices.Template.Api.Common.Errors;
using DomainDrivenVerticalSlices.Template.Api.Common.Persistence;
using DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Domain;
using DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

internal sealed class CreateEntity1Handler(AppDbContext dbContext)
{
    public async Task<Result<CreateEntity1Response>> HandleAsync(
        CreateEntity1Request request,
        CancellationToken cancellationToken)
    {
        bool property1AlreadyExists = await dbContext.Entities1
            .AnyAsync(entity1 => entity1.ValueObject1.Property1 == request.Property1.Trim(), cancellationToken);

        if (property1AlreadyExists)
        {
            return Result<CreateEntity1Response>.Failure(Entity1Errors.Property1AlreadyExists(request.Property1));
        }

        Result<ValueObject1> valueObject1Result = ValueObject1.Create(request.Property1);

        if (valueObject1Result.IsFailure)
        {
            return Result<CreateEntity1Response>.Failure(valueObject1Result.Error);
        }

        Result<Entity1> entity1Result = Entity1.Create(valueObject1Result.Value);

        if (entity1Result.IsFailure)
        {
            return Result<CreateEntity1Response>.Failure(entity1Result.Error);
        }

        Entity1 entity1 = entity1Result.Value;

        dbContext.Entities1.Add(entity1);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateEntity1Response(entity1.Id);
    }
}
