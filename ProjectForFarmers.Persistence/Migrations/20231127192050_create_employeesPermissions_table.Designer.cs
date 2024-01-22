﻿// <auto-generated />
using System;
using ProjectForFarmers.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ProjectForFarmers.Persistence.Migrations
{
    [DbContext(typeof(MainDbContext))]
    [Migration("20231127192050_create_employeesPermissions_table")]
    partial class create_employeesPermissions_table
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Agroforum.Domain.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("character varying(60)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int[]>("Roles")
                        .IsRequired()
                        .HasColumnType("integer[]");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("character varying(60)");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("Accounts", (string)null);
                });

            modelBuilder.Entity("Agroforum.Domain.Address", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("HouseNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Region")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Settlement")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("Addresses", (string)null);
                });

            modelBuilder.Entity("Agroforum.Domain.EmployeePermissions", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uuid");

                    b.Property<bool>("AddEmployee")
                        .HasColumnType("boolean");

                    b.Property<bool>("CreateProduct")
                        .HasColumnType("boolean");

                    b.Property<bool>("DeleteEmployee")
                        .HasColumnType("boolean");

                    b.Property<bool>("DeleteProduct")
                        .HasColumnType("boolean");

                    b.Property<Guid>("FarmId")
                        .HasColumnType("uuid");

                    b.Property<bool>("ListProductForSale")
                        .HasColumnType("boolean");

                    b.Property<bool>("UpdateEmployeePermissions")
                        .HasColumnType("boolean");

                    b.Property<bool>("UpdateFarmInformation")
                        .HasColumnType("boolean");

                    b.Property<bool>("UpdateProduct")
                        .HasColumnType("boolean");

                    b.Property<bool>("WithdrawProductFromSale")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("FarmId");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("EmployeesPermissions", (string)null);
                });

            modelBuilder.Entity("Agroforum.Domain.Farm", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AddressId")
                        .HasColumnType("uuid");

                    b.Property<string>("ContactEmail")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("AddressId")
                        .IsUnique();

                    b.ToTable("Farms", (string)null);
                });

            modelBuilder.Entity("Agroforum.Domain.EmployeePermissions", b =>
                {
                    b.HasOne("Agroforum.Domain.Account", null)
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Agroforum.Domain.Farm", null)
                        .WithMany()
                        .HasForeignKey("FarmId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Agroforum.Domain.Farm", b =>
                {
                    b.HasOne("Agroforum.Domain.Address", null)
                        .WithOne()
                        .HasForeignKey("Agroforum.Domain.Farm", "AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
