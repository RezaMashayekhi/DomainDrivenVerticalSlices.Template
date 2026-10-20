namespace DomainDrivenVerticalSlices.Template.Api.Modules.Entity1;

using DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Features.CreateEntity1;
using DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Features.DeleteEntity1;
using DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Features.GetEntity1ById;
using DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Features.ListEntity1;
using DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Features.UpdateEntity1;
using FluentValidation;

internal static class Entity1Module
{
    public static IServiceCollection AddEntity1Module(this IServiceCollection services)
    {
        services.AddScoped<CreateEntity1Handler>();
        services.AddScoped<GetEntity1ByIdHandler>();
        services.AddScoped<ListEntity1Handler>();
        services.AddScoped<UpdateEntity1Handler>();
        services.AddScoped<DeleteEntity1Handler>();

        services.AddValidatorsFromAssemblyContaining<CreateEntity1Validator>(includeInternalTypes: true);

        return services;
    }
}
