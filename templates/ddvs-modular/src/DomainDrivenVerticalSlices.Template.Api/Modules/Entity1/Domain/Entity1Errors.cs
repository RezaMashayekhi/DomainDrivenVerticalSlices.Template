namespace DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Domain;

using DomainDrivenVerticalSlices.Template.Api.Common.Errors;

internal static class Entity1Errors
{
    public static Error NotFound(Guid id)
    {
        return Error.NotFound(
            "Entity1.NotFound",
            $"The entity1 with ID '{id}' was not found.");
    }

    public static Error Property1AlreadyExists(string property1)
    {
        return Error.Conflict(
            "Entity1.Property1AlreadyExists",
            $"An Entity1 with Property1 '{property1}' already exists.");
    }

    public static Error Property1Required()
    {
        return Error.Validation(new Dictionary<string, string[]>
        {
            ["Property1"] = ["Property1 cannot be empty."],
        });
    }

    public static Error ValueObject1Required()
    {
        return Error.Validation(new Dictionary<string, string[]>
        {
            ["ValueObject1"] = ["ValueObject1 is required."],
        });
    }
}
