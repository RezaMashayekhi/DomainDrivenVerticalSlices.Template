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
            .Where(type => GetReferencedTypes(type).Any(IsForbiddenDomainDependency))
            .ToArray();

        Assert.Empty(violatingTypes);
    }

    [Fact]
    public void EndpointTypes_Should_Not_Depend_On_EfCore_Or_Persistence()
    {
        Type[] violatingTypes = ApiAssembly.GetTypes()
            .Where(type => typeof(IEndpoint).IsAssignableFrom(type) && type is { IsAbstract: false, IsInterface: false })
            .Where(type => GetReferencedTypes(type).Any(IsForbiddenEndpointDependency))
            .ToArray();

        Assert.Empty(violatingTypes);
    }

    [Fact]
    public void Modules_Should_Not_Reference_Another_Module_Infrastructure()
    {
        Type[] violatingTypes = ApiAssembly.GetTypes()
            .Where(type => type.Namespace?.Contains(".Modules.") == true)
            .Where(type => GetReferencedTypes(type).Any(referencedType =>
                IsDifferentModuleInfrastructureReference(type, referencedType)))
            .ToArray();

        Assert.Empty(violatingTypes);
    }

    private static bool IsForbiddenDomainDependency(Type referencedType)
    {
        string? @namespace = referencedType.Namespace;

        return @namespace is not null &&
            (@namespace.StartsWith("Microsoft.AspNetCore", StringComparison.Ordinal) ||
             @namespace.StartsWith("Microsoft.EntityFrameworkCore", StringComparison.Ordinal) ||
             @namespace.Contains(".Infrastructure", StringComparison.Ordinal));
    }

    private static bool IsForbiddenEndpointDependency(Type referencedType)
    {
        string? @namespace = referencedType.Namespace;

        return @namespace is not null &&
            (@namespace.StartsWith("Microsoft.EntityFrameworkCore", StringComparison.Ordinal) ||
             @namespace.Contains(".Common.Persistence", StringComparison.Ordinal));
    }

    private static bool IsDifferentModuleInfrastructureReference(Type sourceType, Type referencedType)
    {
        string? sourceModule = GetModuleName(sourceType);
        string? referencedModule = GetModuleName(referencedType);

        return sourceModule is not null &&
            referencedModule is not null &&
            sourceModule != referencedModule &&
            referencedType.Namespace?.Contains(".Infrastructure", StringComparison.Ordinal) == true;
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

    private static IEnumerable<Type> GetReferencedTypes(Type type)
    {
        foreach (Type interfaceType in type.GetInterfaces())
        {
            yield return interfaceType;
        }

        if (type.BaseType is not null)
        {
            yield return type.BaseType;
        }

        foreach (FieldInfo field in type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
        {
            yield return field.FieldType;
        }

        foreach (PropertyInfo property in type.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
        {
            yield return property.PropertyType;
        }

        foreach (ConstructorInfo constructor in type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        {
            foreach (ParameterInfo parameter in constructor.GetParameters())
            {
                yield return parameter.ParameterType;
            }
        }

        foreach (MethodInfo method in type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
        {
            yield return method.ReturnType;

            foreach (ParameterInfo parameter in method.GetParameters())
            {
                yield return parameter.ParameterType;
            }
        }
    }
}
