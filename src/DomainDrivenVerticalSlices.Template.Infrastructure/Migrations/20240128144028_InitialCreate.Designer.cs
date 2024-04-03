﻿// <auto-generated />
using System;
using DomainDrivenVerticalSlices.Template.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DomainDrivenVerticalSlices.Template.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240128144028_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.1");

            modelBuilder.Entity("DomainDrivenVerticalSlices.Template.Domain.Entities.Entity1", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Entities1");
                });

            modelBuilder.Entity("DomainDrivenVerticalSlices.Template.Domain.Entities.Entity1", b =>
                {
                    b.OwnsOne("DomainDrivenVerticalSlices.Template.Domain.ValueObjects.ValueObject1", "ValueObject1", b1 =>
                        {
                            b1.Property<Guid>("Entity1Id")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Property1")
                                .IsRequired()
                                .HasColumnType("TEXT");

                            b1.HasKey("Entity1Id");

                            b1.ToTable("Entities1");

                            b1.WithOwner()
                                .HasForeignKey("Entity1Id");
                        });

                    b.Navigation("ValueObject1")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
