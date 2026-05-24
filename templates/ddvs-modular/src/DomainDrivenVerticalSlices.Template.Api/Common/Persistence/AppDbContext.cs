namespace DomainDrivenVerticalSlices.Template.Api.Common.Persistence;

using DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Domain;
using Microsoft.EntityFrameworkCore;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options)
{
    public DbSet<Entity1> Entities1 => Set<Entity1>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
