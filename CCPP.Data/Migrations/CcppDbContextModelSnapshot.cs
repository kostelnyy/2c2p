﻿// <auto-generated />
using System;
using CCPP.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CCPP.Data.Migrations
{
    [DbContext(typeof(CcppDbContext))]
    partial class CcppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("transactions")
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CCPP.Core.Domain.PaymentTransaction", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("nvarchar(3)")
                        .HasMaxLength(3);

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Currency");

                    b.HasIndex("Status");

                    b.HasIndex("TransactionDate");

                    b.ToTable("PaymentTranstactions");
                });
#pragma warning restore 612, 618
        }
    }
}
