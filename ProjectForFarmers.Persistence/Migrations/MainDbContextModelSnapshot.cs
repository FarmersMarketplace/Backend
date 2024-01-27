﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ProjectForFarmers.Persistence.DbContexts;

#nullable disable

namespace ProjectForFarmers.Persistence.Migrations
{
    [DbContext(typeof(MainDbContext))]
    partial class MainDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ProjectForFarmers.Domain.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("Accounts", (string)null);
                });

            modelBuilder.Entity("ProjectForFarmers.Domain.Address", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("District")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("HouseNumber")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("character varying(5)");

                    b.Property<string>("Region")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("Settlement")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("Addresses", (string)null);
                });

            modelBuilder.Entity("ProjectForFarmers.Domain.DayOfWeek", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("EndHour")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("character varying(2)");

                    b.Property<string>("EndMinute")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("character varying(2)");

                    b.Property<string>("StartHour")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("character varying(2)");

                    b.Property<string>("StartMinute")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("character varying(2)");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("DaysOfWeek", (string)null);
                });

            modelBuilder.Entity("ProjectForFarmers.Domain.Farm", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AddressId")
                        .HasColumnType("uuid");

                    b.Property<string>("ContactEmail")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ContactPhone")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<List<string>>("ImagesNames")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ScheduleId")
                        .HasColumnType("uuid");

                    b.Property<string>("WebsiteUrl")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AddressId")
                        .IsUnique();

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("OwnerId")
                        .IsUnique();

                    b.HasIndex("ScheduleId")
                        .IsUnique();

                    b.ToTable("Farms", (string)null);
                });

            modelBuilder.Entity("ProjectForFarmers.Domain.MonthStatistic", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("BookedOrdersId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("BookedOrdersStatisticId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CompleteOrdersStatisticId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CompletedOrdersId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("FarmId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("NewOrdersId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("NewOrdersStatisticId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("PreviousOrdersId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("PreviousOrdersStatisticId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ProcessingOrdersId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ProcessingOrdersStatisticId")
                        .HasColumnType("uuid");

                    b.Property<int>("Producer")
                        .HasColumnType("integer");

                    b.Property<Guid>("ProducerId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("TotalActivityId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TotalActivityStatisticId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("TotalRevenue")
                        .HasColumnType("numeric");

                    b.Property<float>("TotalRevenuePercentage")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("BookedOrdersId");

                    b.HasIndex("BookedOrdersStatisticId")
                        .IsUnique();

                    b.HasIndex("CompleteOrdersStatisticId")
                        .IsUnique();

                    b.HasIndex("CompletedOrdersId");

                    b.HasIndex("FarmId");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("NewOrdersId");

                    b.HasIndex("NewOrdersStatisticId")
                        .IsUnique();

                    b.HasIndex("PreviousOrdersId");

                    b.HasIndex("PreviousOrdersStatisticId")
                        .IsUnique();

                    b.HasIndex("ProcessingOrdersId");

                    b.HasIndex("ProcessingOrdersStatisticId")
                        .IsUnique();

                    b.HasIndex("TotalActivityId");

                    b.HasIndex("TotalActivityStatisticId")
                        .IsUnique();

                    b.ToTable("MonthesStatistics", (string)null);
                });

            modelBuilder.Entity("ProjectForFarmers.Domain.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CustomerEmail")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid");

                    b.Property<string>("CustomerName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CustomerPhone")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("FarmId")
                        .HasColumnType("uuid");

                    b.Property<long>("Number")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(7)
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseSerialColumn(b.Property<long>("Number"));

                    b.Property<decimal>("PaymentTotal")
                        .HasColumnType("numeric");

                    b.Property<int>("PaymentType")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("FarmId");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("Orders", (string)null);
                });

            modelBuilder.Entity("ProjectForFarmers.Domain.OrderGroupStatistic", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<long>("Count")
                        .HasColumnType("bigint");

                    b.Property<float>("PercentageChange")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.ToTable("OrderGroupStatistic");
                });

            modelBuilder.Entity("ProjectForFarmers.Domain.Schedule", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("FridayId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("MondayId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SaturdayId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SundayId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ThursdayId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TuesdayId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("WednesdayId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("FridayId")
                        .IsUnique();

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("MondayId")
                        .IsUnique();

                    b.HasIndex("SaturdayId")
                        .IsUnique();

                    b.HasIndex("SundayId")
                        .IsUnique();

                    b.HasIndex("ThursdayId")
                        .IsUnique();

                    b.HasIndex("TuesdayId")
                        .IsUnique();

                    b.HasIndex("WednesdayId")
                        .IsUnique();

                    b.ToTable("Schedules", (string)null);
                });

            modelBuilder.Entity("ProjectForFarmers.Domain.Farm", b =>
                {
                    b.HasOne("ProjectForFarmers.Domain.Address", "Address")
                        .WithOne()
                        .HasForeignKey("ProjectForFarmers.Domain.Farm", "AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectForFarmers.Domain.Account", "Owner")
                        .WithOne()
                        .HasForeignKey("ProjectForFarmers.Domain.Farm", "OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectForFarmers.Domain.Schedule", "Schedule")
                        .WithOne()
                        .HasForeignKey("ProjectForFarmers.Domain.Farm", "ScheduleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");

                    b.Navigation("Owner");

                    b.Navigation("Schedule");
                });

            modelBuilder.Entity("ProjectForFarmers.Domain.MonthStatistic", b =>
                {
                    b.HasOne("ProjectForFarmers.Domain.OrderGroupStatistic", "BookedOrders")
                        .WithMany()
                        .HasForeignKey("BookedOrdersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectForFarmers.Domain.OrderGroupStatistic", null)
                        .WithOne()
                        .HasForeignKey("ProjectForFarmers.Domain.MonthStatistic", "BookedOrdersStatisticId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectForFarmers.Domain.OrderGroupStatistic", null)
                        .WithOne()
                        .HasForeignKey("ProjectForFarmers.Domain.MonthStatistic", "CompleteOrdersStatisticId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectForFarmers.Domain.OrderGroupStatistic", "CompletedOrders")
                        .WithMany()
                        .HasForeignKey("CompletedOrdersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectForFarmers.Domain.Farm", null)
                        .WithMany("Dashboard")
                        .HasForeignKey("FarmId");

                    b.HasOne("ProjectForFarmers.Domain.OrderGroupStatistic", "NewOrders")
                        .WithMany()
                        .HasForeignKey("NewOrdersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectForFarmers.Domain.OrderGroupStatistic", null)
                        .WithOne()
                        .HasForeignKey("ProjectForFarmers.Domain.MonthStatistic", "NewOrdersStatisticId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectForFarmers.Domain.OrderGroupStatistic", "PreviousOrders")
                        .WithMany()
                        .HasForeignKey("PreviousOrdersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectForFarmers.Domain.OrderGroupStatistic", null)
                        .WithOne()
                        .HasForeignKey("ProjectForFarmers.Domain.MonthStatistic", "PreviousOrdersStatisticId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectForFarmers.Domain.OrderGroupStatistic", "ProcessingOrders")
                        .WithMany()
                        .HasForeignKey("ProcessingOrdersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectForFarmers.Domain.OrderGroupStatistic", null)
                        .WithOne()
                        .HasForeignKey("ProjectForFarmers.Domain.MonthStatistic", "ProcessingOrdersStatisticId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectForFarmers.Domain.OrderGroupStatistic", "TotalActivity")
                        .WithMany()
                        .HasForeignKey("TotalActivityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectForFarmers.Domain.OrderGroupStatistic", null)
                        .WithOne()
                        .HasForeignKey("ProjectForFarmers.Domain.MonthStatistic", "TotalActivityStatisticId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BookedOrders");

                    b.Navigation("CompletedOrders");

                    b.Navigation("NewOrders");

                    b.Navigation("PreviousOrders");

                    b.Navigation("ProcessingOrders");

                    b.Navigation("TotalActivity");
                });

            modelBuilder.Entity("ProjectForFarmers.Domain.Order", b =>
                {
                    b.HasOne("ProjectForFarmers.Domain.Account", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectForFarmers.Domain.Farm", "Farm")
                        .WithMany()
                        .HasForeignKey("FarmId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Farm");
                });

            modelBuilder.Entity("ProjectForFarmers.Domain.Schedule", b =>
                {
                    b.HasOne("ProjectForFarmers.Domain.DayOfWeek", "Friday")
                        .WithOne()
                        .HasForeignKey("ProjectForFarmers.Domain.Schedule", "FridayId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectForFarmers.Domain.DayOfWeek", "Monday")
                        .WithOne()
                        .HasForeignKey("ProjectForFarmers.Domain.Schedule", "MondayId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectForFarmers.Domain.DayOfWeek", "Saturday")
                        .WithOne()
                        .HasForeignKey("ProjectForFarmers.Domain.Schedule", "SaturdayId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectForFarmers.Domain.DayOfWeek", "Sunday")
                        .WithOne()
                        .HasForeignKey("ProjectForFarmers.Domain.Schedule", "SundayId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectForFarmers.Domain.DayOfWeek", "Thursday")
                        .WithOne()
                        .HasForeignKey("ProjectForFarmers.Domain.Schedule", "ThursdayId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectForFarmers.Domain.DayOfWeek", "Tuesday")
                        .WithOne()
                        .HasForeignKey("ProjectForFarmers.Domain.Schedule", "TuesdayId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectForFarmers.Domain.DayOfWeek", "Wednesday")
                        .WithOne()
                        .HasForeignKey("ProjectForFarmers.Domain.Schedule", "WednesdayId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Friday");

                    b.Navigation("Monday");

                    b.Navigation("Saturday");

                    b.Navigation("Sunday");

                    b.Navigation("Thursday");

                    b.Navigation("Tuesday");

                    b.Navigation("Wednesday");
                });

            modelBuilder.Entity("ProjectForFarmers.Domain.Farm", b =>
                {
                    b.Navigation("Dashboard");
                });
#pragma warning restore 612, 618
        }
    }
}
