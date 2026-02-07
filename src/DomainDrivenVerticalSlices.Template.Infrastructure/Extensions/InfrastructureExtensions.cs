namespace DomainDrivenVerticalSlices.Template.Infrastructure.Extensions;

using DomainDrivenVerticalSlices.Template.Application.Interfaces;
using DomainDrivenVerticalSlices.Template.Infrastructure.Data;
using DomainDrivenVerticalSlices.Template.Infrastructure.Data.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Register TimeProvider for audit timestamps
        services.AddSingleton(TimeProvider.System);

        // Register EF Core interceptors
        services.AddScoped<AuditableEntityInterceptor>();
        services.AddScoped<DispatchDomainEventsInterceptor>();

        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            options.AddInterceptors(
                sp.GetRequiredService<AuditableEntityInterceptor>(),
                sp.GetRequiredService<DispatchDomainEventsInterceptor>());

            options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IEntity1Repository, Entity1Repository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
