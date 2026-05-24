namespace DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Features.CreateEntity1;

internal static class CreateEntity1Mapping
{
    public static string ToLocation(this CreateEntity1Response response)
    {
        return $"/api/entity1/{response.Id}";
    }
}
