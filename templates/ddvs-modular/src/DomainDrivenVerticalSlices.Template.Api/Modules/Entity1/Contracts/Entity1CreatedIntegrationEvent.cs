namespace DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Contracts;

public sealed record Entity1CreatedIntegrationEvent(Guid Entity1Id, string Property1, DateTime CreatedAtUtc);
