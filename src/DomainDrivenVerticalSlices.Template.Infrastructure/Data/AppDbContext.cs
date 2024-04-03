namespace DomainDrivenVerticalSlices.Template.Infrastructure.Data;

using DomainDrivenVerticalSlices.Template.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options)
{
    public DbSet<Entity1> Entities1 { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Entity1>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.OwnsOne(e => e.ValueObject1);
        });
    }
}
