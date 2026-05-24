namespace DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Features.DeleteEntity1;

using DomainDrivenVerticalSlices.Template.Api.Common.Errors;
using DomainDrivenVerticalSlices.Template.Api.Common.Persistence;
using DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Domain;
using Microsoft.EntityFrameworkCore;

internal sealed class DeleteEntity1Handler(AppDbContext dbContext)
{
    public async Task<Result> HandleAsync(
        DeleteEntity1Request request,
        CancellationToken cancellationToken)
    {
        Entity1? entity1 = await dbContext.Entities1
            .FirstOrDefaultAsync(entity1 => entity1.Id == request.Id, cancellationToken);

        if (entity1 is null)
        {
            return Result.Failure(Entity1Errors.NotFound(request.Id));
        }

        dbContext.Entities1.Remove(entity1);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
