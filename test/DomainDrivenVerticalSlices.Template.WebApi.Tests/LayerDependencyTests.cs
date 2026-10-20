namespace DomainDrivenVerticalSlices.Template.WebApi.Tests;

using System.Reflection;
using DomainDrivenVerticalSlices.Template.Application.Dtos;
using DomainDrivenVerticalSlices.Template.Domain.Entities;

public class LayerDependencyTests
{
    [Fact]
    public void Domain_And_Application_DoNotReferenceOuterLayers()
    {
        foreach (Assembly assembly in new[] { typeof(Entity1).Assembly, typeof(Entity1Dto).Assembly })
        {
            Assert.DoesNotContain(assembly.GetReferencedAssemblies(), reference =>
                reference.Name!.Contains(".Infrastructure", StringComparison.Ordinal) ||
                reference.Name.Contains(".WebApi", StringComparison.Ordinal) ||
                reference.Name.StartsWith("Microsoft.EntityFrameworkCore", StringComparison.Ordinal) ||
                reference.Name.StartsWith("Microsoft.AspNetCore", StringComparison.Ordinal));
        }

        Assert.DoesNotContain(typeof(Entity1).Assembly.GetReferencedAssemblies(), reference =>
            reference.Name!.Contains(".Application", StringComparison.Ordinal));
    }
}
