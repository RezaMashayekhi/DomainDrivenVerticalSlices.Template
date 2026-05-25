namespace DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Infrastructure;

using DomainDrivenVerticalSlices.Template.Api.Modules.Entity1.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class Entity1Configuration : IEntityTypeConfiguration<Entity1>
{
    public void Configure(EntityTypeBuilder<Entity1> builder)
    {
        builder.ToTable("entity1");

        builder.HasKey(entity1 => entity1.Id);

        builder.OwnsOne(entity1 => entity1.ValueObject1, valueObject =>
        {
            valueObject.Property(valueObject1 => valueObject1.Property1)
                .HasColumnName("Property1")
                .HasMaxLength(200)
                .IsRequired();

            valueObject.HasIndex(valueObject1 => valueObject1.Property1)
                .IsUnique();
        });

        builder.Navigation(entity1 => entity1.ValueObject1)
            .IsRequired();

        builder.Property(entity1 => entity1.CreatedAtUtc)
            .IsRequired();

        builder.Ignore(entity1 => entity1.DomainEvents);
    }
}
