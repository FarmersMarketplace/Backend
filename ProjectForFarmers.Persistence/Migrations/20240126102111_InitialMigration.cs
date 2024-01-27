using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ProjectForFarmers.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Surname = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Region = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    District = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Settlement = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Street = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    HouseNumber = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    PostalCode = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    Note = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DaysOfWeek",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StartHour = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    StartMinute = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    EndHour = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    EndMinute = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DaysOfWeek", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderGroupStatistic",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Count = table.Column<long>(type: "bigint", nullable: false),
                    PercentageChange = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderGroupStatistic", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MondayId = table.Column<Guid>(type: "uuid", nullable: false),
                    TuesdayId = table.Column<Guid>(type: "uuid", nullable: false),
                    WednesdayId = table.Column<Guid>(type: "uuid", nullable: false),
                    ThursdayId = table.Column<Guid>(type: "uuid", nullable: false),
                    FridayId = table.Column<Guid>(type: "uuid", nullable: false),
                    SaturdayId = table.Column<Guid>(type: "uuid", nullable: false),
                    SundayId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schedules_DaysOfWeek_FridayId",
                        column: x => x.FridayId,
                        principalTable: "DaysOfWeek",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Schedules_DaysOfWeek_MondayId",
                        column: x => x.MondayId,
                        principalTable: "DaysOfWeek",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Schedules_DaysOfWeek_SaturdayId",
                        column: x => x.SaturdayId,
                        principalTable: "DaysOfWeek",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Schedules_DaysOfWeek_SundayId",
                        column: x => x.SundayId,
                        principalTable: "DaysOfWeek",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Schedules_DaysOfWeek_ThursdayId",
                        column: x => x.ThursdayId,
                        principalTable: "DaysOfWeek",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Schedules_DaysOfWeek_TuesdayId",
                        column: x => x.TuesdayId,
                        principalTable: "DaysOfWeek",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Schedules_DaysOfWeek_WednesdayId",
                        column: x => x.WednesdayId,
                        principalTable: "DaysOfWeek",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Farms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ContactEmail = table.Column<string>(type: "text", nullable: false),
                    ContactPhone = table.Column<string>(type: "text", nullable: false),
                    WebsiteUrl = table.Column<string>(type: "text", nullable: true),
                    ImagesNames = table.Column<List<string>>(type: "text[]", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    AddressId = table.Column<Guid>(type: "uuid", nullable: false),
                    ScheduleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Farms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Farms_Accounts_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Farms_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Farms_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MonthesStatistics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProducerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Producer = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BookedOrdersId = table.Column<Guid>(type: "uuid", nullable: false),
                    BookedOrdersStatisticId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompletedOrdersId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompleteOrdersStatisticId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProcessingOrdersId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProcessingOrdersStatisticId = table.Column<Guid>(type: "uuid", nullable: false),
                    NewOrdersId = table.Column<Guid>(type: "uuid", nullable: false),
                    NewOrdersStatisticId = table.Column<Guid>(type: "uuid", nullable: false),
                    PreviousOrdersId = table.Column<Guid>(type: "uuid", nullable: false),
                    PreviousOrdersStatisticId = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalActivityId = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalActivityStatisticId = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalRevenue = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalRevenuePercentage = table.Column<float>(type: "real", nullable: false),
                    FarmId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthesStatistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonthesStatistics_Farms_FarmId",
                        column: x => x.FarmId,
                        principalTable: "Farms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MonthesStatistics_OrderGroupStatistic_BookedOrdersId",
                        column: x => x.BookedOrdersId,
                        principalTable: "OrderGroupStatistic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthesStatistics_OrderGroupStatistic_BookedOrdersStatistic~",
                        column: x => x.BookedOrdersStatisticId,
                        principalTable: "OrderGroupStatistic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthesStatistics_OrderGroupStatistic_CompleteOrdersStatist~",
                        column: x => x.CompleteOrdersStatisticId,
                        principalTable: "OrderGroupStatistic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthesStatistics_OrderGroupStatistic_CompletedOrdersId",
                        column: x => x.CompletedOrdersId,
                        principalTable: "OrderGroupStatistic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthesStatistics_OrderGroupStatistic_NewOrdersId",
                        column: x => x.NewOrdersId,
                        principalTable: "OrderGroupStatistic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthesStatistics_OrderGroupStatistic_NewOrdersStatisticId",
                        column: x => x.NewOrdersStatisticId,
                        principalTable: "OrderGroupStatistic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthesStatistics_OrderGroupStatistic_PreviousOrdersId",
                        column: x => x.PreviousOrdersId,
                        principalTable: "OrderGroupStatistic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthesStatistics_OrderGroupStatistic_PreviousOrdersStatist~",
                        column: x => x.PreviousOrdersStatisticId,
                        principalTable: "OrderGroupStatistic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthesStatistics_OrderGroupStatistic_ProcessingOrdersId",
                        column: x => x.ProcessingOrdersId,
                        principalTable: "OrderGroupStatistic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthesStatistics_OrderGroupStatistic_ProcessingOrdersStati~",
                        column: x => x.ProcessingOrdersStatisticId,
                        principalTable: "OrderGroupStatistic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthesStatistics_OrderGroupStatistic_TotalActivityId",
                        column: x => x.TotalActivityId,
                        principalTable: "OrderGroupStatistic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthesStatistics_OrderGroupStatistic_TotalActivityStatisti~",
                        column: x => x.TotalActivityStatisticId,
                        principalTable: "OrderGroupStatistic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Number = table.Column<long>(type: "bigint", maxLength: 7, nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CustomerName = table.Column<string>(type: "text", nullable: false),
                    CustomerPhone = table.Column<string>(type: "text", nullable: false),
                    CustomerEmail = table.Column<string>(type: "text", nullable: false),
                    PaymentTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    PaymentType = table.Column<int>(type: "integer", nullable: false),
                    FarmId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Accounts_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Farms_FarmId",
                        column: x => x.FarmId,
                        principalTable: "Farms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Id",
                table: "Accounts",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_Id",
                table: "Addresses",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DaysOfWeek_Id",
                table: "DaysOfWeek",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Farms_AddressId",
                table: "Farms",
                column: "AddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Farms_Id",
                table: "Farms",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Farms_OwnerId",
                table: "Farms",
                column: "OwnerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Farms_ScheduleId",
                table: "Farms",
                column: "ScheduleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MonthesStatistics_BookedOrdersId",
                table: "MonthesStatistics",
                column: "BookedOrdersId");

            migrationBuilder.CreateIndex(
                name: "IX_MonthesStatistics_BookedOrdersStatisticId",
                table: "MonthesStatistics",
                column: "BookedOrdersStatisticId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MonthesStatistics_CompletedOrdersId",
                table: "MonthesStatistics",
                column: "CompletedOrdersId");

            migrationBuilder.CreateIndex(
                name: "IX_MonthesStatistics_CompleteOrdersStatisticId",
                table: "MonthesStatistics",
                column: "CompleteOrdersStatisticId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MonthesStatistics_FarmId",
                table: "MonthesStatistics",
                column: "FarmId");

            migrationBuilder.CreateIndex(
                name: "IX_MonthesStatistics_Id",
                table: "MonthesStatistics",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MonthesStatistics_NewOrdersId",
                table: "MonthesStatistics",
                column: "NewOrdersId");

            migrationBuilder.CreateIndex(
                name: "IX_MonthesStatistics_NewOrdersStatisticId",
                table: "MonthesStatistics",
                column: "NewOrdersStatisticId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MonthesStatistics_PreviousOrdersId",
                table: "MonthesStatistics",
                column: "PreviousOrdersId");

            migrationBuilder.CreateIndex(
                name: "IX_MonthesStatistics_PreviousOrdersStatisticId",
                table: "MonthesStatistics",
                column: "PreviousOrdersStatisticId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MonthesStatistics_ProcessingOrdersId",
                table: "MonthesStatistics",
                column: "ProcessingOrdersId");

            migrationBuilder.CreateIndex(
                name: "IX_MonthesStatistics_ProcessingOrdersStatisticId",
                table: "MonthesStatistics",
                column: "ProcessingOrdersStatisticId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MonthesStatistics_TotalActivityId",
                table: "MonthesStatistics",
                column: "TotalActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_MonthesStatistics_TotalActivityStatisticId",
                table: "MonthesStatistics",
                column: "TotalActivityStatisticId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_FarmId",
                table: "Orders",
                column: "FarmId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Id",
                table: "Orders",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_FridayId",
                table: "Schedules",
                column: "FridayId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_Id",
                table: "Schedules",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_MondayId",
                table: "Schedules",
                column: "MondayId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_SaturdayId",
                table: "Schedules",
                column: "SaturdayId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_SundayId",
                table: "Schedules",
                column: "SundayId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_ThursdayId",
                table: "Schedules",
                column: "ThursdayId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_TuesdayId",
                table: "Schedules",
                column: "TuesdayId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_WednesdayId",
                table: "Schedules",
                column: "WednesdayId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MonthesStatistics");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "OrderGroupStatistic");

            migrationBuilder.DropTable(
                name: "Farms");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropTable(
                name: "DaysOfWeek");
        }
    }
}
