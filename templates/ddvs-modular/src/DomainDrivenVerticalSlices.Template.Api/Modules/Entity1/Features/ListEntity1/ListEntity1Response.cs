namespace DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Features.ListEntity1;

using DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Contracts;

public sealed record ListEntity1Response(IReadOnlyCollection<Entity1Dto> Items, int Page, int PageSize, int TotalCount);
