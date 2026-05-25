namespace DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Features.ListEntity1;

using DomainDrivenVerticalSlices.Template.Api.Common.Errors;
using DomainDrivenVerticalSlices.Template.Api.Common.Persistence;
using DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Contracts;
using Microsoft.EntityFrameworkCore;

internal sealed class ListEntity1Handler(AppDbContext dbContext)
{
    public async Task<Result<ListEntity1Response>> HandleAsync(
        ListEntity1Request request,
        CancellationToken cancellationToken)
    {
        int page = Math.Max(request.Page, 1);
        int pageSize = Math.Clamp(request.PageSize, 1, 100);

        int totalCount = await dbContext.Entities1.CountAsync(cancellationToken);

        Entity1Dto[] items = await dbContext.Entities1
            .OrderBy(entity1 => entity1.ValueObject1.Property1)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(entity1 => new Entity1Dto(
                entity1.Id,
                new ValueObject1Dto(entity1.ValueObject1.Property1)))
            .ToArrayAsync(cancellationToken);

        return new ListEntity1Response(items, page, pageSize, totalCount);
    }
}
