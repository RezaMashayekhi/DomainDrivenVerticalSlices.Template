namespace DomainDrivenVerticalSlices.Template.ArchitectureTests;

using System.Reflection;
using DomainDrivenVerticalSlices.Template.Api.Common.Endpoints;

public sealed class DependencyRulesTests
{
    private static readonly Assembly ApiAssembly = typeof(Program).Assembly;

    [Fact]
    public void DomainTypes_Should_Not_Depend_On_AspNetCore_EfCore_Or_Infrastructure()
    {
        Type[] violatingTypes = ApiAssembly.GetTypes()
            .Where(type => type.Namespace?.Contains(".Modules.") == true)
            .Where(type => type.Namespace?.EndsWith(".Domain") == true ||
                type.Namespace?.Contains(".Domain.") == true)
            .Where(type => DependencyInspector.GetReferencedTypes(type).Any(IsForbiddenDomainDependency))
            .ToArray();

        Assert.Empty(violatingTypes);
    }

    [Fact]
    public void EndpointTypes_Should_Not_Depend_On_EfCore_Or_Persistence()
    {
        Type[] violatingTypes = ApiAssembly.GetTypes()
            .Where(type => typeof(IEndpoint).IsAssignableFrom(type) && type is { IsAbstract: false, IsInterface: false })
            .Where(type => DependencyInspector.GetReferencedTypes(type).Any(IsForbiddenEndpointDependency))
            .ToArray();

        Assert.Empty(violatingTypes);
    }

    [Fact]
    public void Modules_Should_Reference_Other_Modules_Only_Through_Contracts()
    {
        Type[] violatingTypes = ApiAssembly.GetTypes()
            .Where(type => type.Namespace?.Contains(".Modules.") == true)
            .Where(type => DependencyInspector.GetReferencedTypes(type).Any(referencedType =>
                IsForbiddenCrossModuleReference(type, referencedType)))
            .ToArray();

        Assert.Empty(violatingTypes);
    }

    private static bool IsForbiddenDomainDependency(Type referencedType)
    {
        string? @namespace = referencedType.Namespace;

        return @namespace is not null &&
            (@namespace.StartsWith("Microsoft.AspNetCore", StringComparison.Ordinal) ||
             @namespace.StartsWith("Microsoft.EntityFrameworkCore", StringComparison.Ordinal) ||
             @namespace.Contains(".Infrastructure", StringComparison.Ordinal) ||
             @namespace.Contains(".Common.Persistence", StringComparison.Ordinal));
    }

    private static bool IsForbiddenEndpointDependency(Type referencedType)
    {
        string? @namespace = referencedType.Namespace;

        return @namespace is not null &&
            (@namespace.StartsWith("Microsoft.EntityFrameworkCore", StringComparison.Ordinal) ||
             @namespace.Contains(".Common.Persistence", StringComparison.Ordinal));
    }

    private static bool IsForbiddenCrossModuleReference(Type sourceType, Type referencedType)
    {
        string? sourceModule = GetModuleName(sourceType);
        string? referencedModule = GetModuleName(referencedType);

        return sourceModule is not null &&
            referencedModule is not null &&
            sourceModule != referencedModule &&
            !((referencedType.Namespace ?? string.Empty) + ".").Contains($".Modules.{referencedModule}.Contracts.", StringComparison.Ordinal);
    }

    private static string? GetModuleName(Type type)
    {
        string? @namespace = type.Namespace;

        if (@namespace is null)
        {
            return null;
        }

        string marker = ".Modules.";
        int markerIndex = @namespace.IndexOf(marker, StringComparison.Ordinal);

        if (markerIndex < 0)
        {
            return null;
        }

        string afterMarker = @namespace[(markerIndex + marker.Length)..];
        int nextDotIndex = afterMarker.IndexOf('.', StringComparison.Ordinal);

        return nextDotIndex < 0 ? afterMarker : afterMarker[..nextDotIndex];
    }

    [Fact]
    public void Inspector_Detects_MethodBody_And_Generic_Dependencies()
    {
        Type secret = typeof(BoundaryFixtures.Modules.Second.Domain.Secret);
        Assert.Contains(secret, DependencyInspector.GetReferencedTypes(typeof(BoundaryFixtures.Modules.First.Features.BodyOnlyDependency)));
        Assert.Contains(secret, DependencyInspector.GetReferencedTypes(typeof(BoundaryFixtures.Modules.First.Features.GenericDependency)));
        Assert.True(IsForbiddenCrossModuleReference(typeof(BoundaryFixtures.Modules.First.Features.BodyOnlyDependency), secret));
        Assert.False(IsForbiddenCrossModuleReference(typeof(BoundaryFixtures.Modules.First.Features.BodyOnlyDependency), typeof(BoundaryFixtures.Modules.Second.Contracts.PublicContract)));
    }
}
