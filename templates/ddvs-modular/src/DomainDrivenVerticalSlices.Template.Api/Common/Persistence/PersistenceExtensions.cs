namespace DomainDrivenVerticalSlices.Template.Api.Common.Persistence;

using Microsoft.EntityFrameworkCore;

public static class PersistenceExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection must be configured.");

        services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));
        return services;
    }
}
