namespace DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Features.UpdateEntity1;

using DomainDrivenVerticalSlices.Template.Api.Common.Errors;
using DomainDrivenVerticalSlices.Template.Api.Common.Persistence;
using DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Contracts;
using DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Domain;
using DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

internal sealed class UpdateEntity1Handler(AppDbContext dbContext)
{
    public async Task<Result<UpdateEntity1Response>> HandleAsync(
        Guid id,
        UpdateEntity1Request request,
        CancellationToken cancellationToken)
    {
        Entity1? entity1 = await dbContext.Entities1
            .FirstOrDefaultAsync(entity1 => entity1.Id == id, cancellationToken);

        if (entity1 is null)
        {
            return Result<UpdateEntity1Response>.Failure(Entity1Errors.NotFound(id));
        }

        Result<ValueObject1> valueObject1Result = ValueObject1.Create(request.Property1);

        if (valueObject1Result.IsFailure)
        {
            return Result<UpdateEntity1Response>.Failure(valueObject1Result.Error);
        }

        ValueObject1 valueObject1 = valueObject1Result.Value;

        bool property1AlreadyExists = await dbContext.Entities1
            .AnyAsync(entity1 =>
                entity1.Id != id &&
                entity1.ValueObject1.Property1 == valueObject1.Property1,
                cancellationToken);

        if (property1AlreadyExists)
        {
            return Result<UpdateEntity1Response>.Failure(Entity1Errors.Property1AlreadyExists(valueObject1.Property1));
        }

        Result updateResult = entity1.Update(valueObject1);

        if (updateResult.IsFailure)
        {
            return Result<UpdateEntity1Response>.Failure(updateResult.Error);
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        Entity1Dto dto = new(entity1.Id, new ValueObject1Dto(entity1.ValueObject1.Property1));

        return new UpdateEntity1Response(dto);
    }
}
