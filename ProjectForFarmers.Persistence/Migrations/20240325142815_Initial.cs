using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FarmersMarketplace.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerAddresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Region = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    District = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Settlement = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Street = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    HouseNumber = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    PostalCode = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Apartment = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerAddresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerPaymentData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CardNumber = table.Column<string>(type: "text", nullable: true),
                    CardExpirationYear = table.Column<string>(type: "text", nullable: true),
                    CardExpirationMonth = table.Column<string>(type: "text", nullable: true),
                    CVV = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerPaymentData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DaysOfWeek",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsOpened = table.Column<bool>(type: "boolean", nullable: false),
                    StartHour = table.Column<byte>(type: "smallint", nullable: true),
                    StartMinute = table.Column<byte>(type: "smallint", nullable: true),
                    EndHour = table.Column<byte>(type: "smallint", nullable: true),
                    EndMinute = table.Column<byte>(type: "smallint", nullable: true)
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
                name: "ProducerAddresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Region = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    District = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Settlement = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Street = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    HouseNumber = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    PostalCode = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProducerAddresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProducerPaymentData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CardNumber = table.Column<string>(type: "text", nullable: true),
                    AccountNumber = table.Column<string>(type: "text", nullable: true),
                    BankUSREOU = table.Column<string>(type: "text", nullable: true),
                    BIC = table.Column<string>(type: "text", nullable: true),
                    HolderFullName = table.Column<string>(type: "text", nullable: true),
                    CardExpirationYear = table.Column<string>(type: "text", nullable: true),
                    CardExpirationMonth = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProducerPaymentData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subcategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subcategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subcategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
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
                    ReceiveDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TotalPayment = table.Column<decimal>(type: "numeric", nullable: false),
                    PaymentType = table.Column<int>(type: "integer", nullable: false),
                    PaymentStatus = table.Column<int>(type: "integer", nullable: false),
                    ReceivingType = table.Column<int>(type: "integer", nullable: false),
                    DeliveryPointId = table.Column<Guid>(type: "uuid", nullable: true),
                    Producer = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ProducerId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_CustomerAddresses_DeliveryPointId",
                        column: x => x.DeliveryPointId,
                        principalTable: "CustomerAddresses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Surname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Patronymic = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    AdditionalPhone = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true),
                    Gender = table.Column<int>(type: "integer", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AvatarName = table.Column<string>(type: "text", nullable: true),
                    PaymentDataId = table.Column<Guid>(type: "uuid", nullable: true),
                    AddressId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_CustomerAddresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "CustomerAddresses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Customers_CustomerPaymentData_PaymentDataId",
                        column: x => x.PaymentDataId,
                        principalTable: "CustomerPaymentData",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MondayId = table.Column<Guid>(type: "uuid", nullable: true),
                    TuesdayId = table.Column<Guid>(type: "uuid", nullable: true),
                    WednesdayId = table.Column<Guid>(type: "uuid", nullable: true),
                    ThursdayId = table.Column<Guid>(type: "uuid", nullable: true),
                    FridayId = table.Column<Guid>(type: "uuid", nullable: true),
                    SaturdayId = table.Column<Guid>(type: "uuid", nullable: false),
                    SundayId = table.Column<Guid>(type: "uuid", nullable: true)
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
                name: "Farmers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Surname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Patronymic = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    AdditionalPhone = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true),
                    Gender = table.Column<int>(type: "integer", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AvatarName = table.Column<string>(type: "text", nullable: true),
                    PaymentDataId = table.Column<Guid>(type: "uuid", nullable: true),
                    AddressId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Farmers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Farmers_ProducerPaymentData_PaymentDataId",
                        column: x => x.PaymentDataId,
                        principalTable: "ProducerPaymentData",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrdersItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    Count = table.Column<long>(type: "bigint", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdersItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrdersItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Farms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ContactEmail = table.Column<string>(type: "text", nullable: true),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    AdditionalPhone = table.Column<string>(type: "text", nullable: true),
                    FirstSocialPageUrl = table.Column<string>(type: "text", nullable: true),
                    SecondSocialPageUrl = table.Column<string>(type: "text", nullable: true),
                    ImagesNames = table.Column<List<string>>(type: "text[]", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    AddressId = table.Column<Guid>(type: "uuid", nullable: false),
                    ScheduleId = table.Column<Guid>(type: "uuid", nullable: false),
                    Categories = table.Column<List<Guid>>(type: "uuid[]", nullable: true),
                    Subcategories = table.Column<List<Guid>>(type: "uuid[]", nullable: true),
                    ReceivingMethods = table.Column<int[]>(type: "integer[]", nullable: true),
                    PaymentTypes = table.Column<int[]>(type: "integer[]", nullable: true),
                    PaymentDataId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Farms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Farms_ProducerPaymentData_PaymentDataId",
                        column: x => x.PaymentDataId,
                        principalTable: "ProducerPaymentData",
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
                name: "Sellers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Surname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Patronymic = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    AdditionalPhone = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true),
                    Gender = table.Column<int>(type: "integer", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ImagesNames = table.Column<List<string>>(type: "text[]", nullable: true),
                    PaymentDataId = table.Column<Guid>(type: "uuid", nullable: true),
                    AddressId = table.Column<Guid>(type: "uuid", nullable: true),
                    ScheduleId = table.Column<Guid>(type: "uuid", nullable: true),
                    FirstSocialPageUrl = table.Column<string>(type: "text", nullable: true),
                    SecondSocialPageUrl = table.Column<string>(type: "text", nullable: true),
                    ReceivingMethods = table.Column<int[]>(type: "integer[]", nullable: true),
                    PaymentTypes = table.Column<int[]>(type: "integer[]", nullable: true),
                    Categories = table.Column<List<Guid>>(type: "uuid[]", nullable: true),
                    Subcategories = table.Column<List<Guid>>(type: "uuid[]", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sellers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sellers_ProducerPaymentData_PaymentDataId",
                        column: x => x.PaymentDataId,
                        principalTable: "ProducerPaymentData",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Sellers_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FarmsLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FarmId = table.Column<Guid>(type: "uuid", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    PropertyName = table.Column<string>(type: "text", nullable: true),
                    Parameters = table.Column<string[]>(type: "text[]", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FarmsLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FarmsLogs_Farms_FarmId",
                        column: x => x.FarmId,
                        principalTable: "Farms",
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
                    BookedOrdersStatisticId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompletedOrdersStatisticId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProcessingOrdersStatisticId = table.Column<Guid>(type: "uuid", nullable: false),
                    NewOrdersStatisticId = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalActivityStatisticId = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalRevenue = table.Column<decimal>(type: "numeric", nullable: false),
                    CustomerWithHighestPaymentId = table.Column<Guid>(type: "uuid", nullable: true),
                    TotalRevenueChangePercentage = table.Column<float>(type: "real", nullable: false),
                    HighestCustomerPayment = table.Column<decimal>(type: "numeric", nullable: false),
                    HighestCustomerPaymentPercentage = table.Column<float>(type: "real", nullable: false),
                    FarmId = table.Column<Guid>(type: "uuid", nullable: true),
                    SellerId = table.Column<Guid>(type: "uuid", nullable: true)
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
                        name: "FK_MonthesStatistics_OrderGroupStatistic_BookedOrdersStatistic~",
                        column: x => x.BookedOrdersStatisticId,
                        principalTable: "OrderGroupStatistic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthesStatistics_OrderGroupStatistic_CompletedOrdersStatis~",
                        column: x => x.CompletedOrdersStatisticId,
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
                        name: "FK_MonthesStatistics_OrderGroupStatistic_ProcessingOrdersStati~",
                        column: x => x.ProcessingOrdersStatisticId,
                        principalTable: "OrderGroupStatistic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthesStatistics_OrderGroupStatistic_TotalActivityStatisti~",
                        column: x => x.TotalActivityStatisticId,
                        principalTable: "OrderGroupStatistic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthesStatistics_Sellers_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Sellers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ArticleNumber = table.Column<string>(type: "text", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubcategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Producer = table.Column<int>(type: "integer", nullable: false),
                    ProducerId = table.Column<Guid>(type: "uuid", nullable: false),
                    PackagingType = table.Column<string>(type: "text", nullable: true),
                    UnitOfMeasurement = table.Column<string>(type: "text", nullable: false),
                    PricePerOne = table.Column<decimal>(type: "numeric", nullable: false),
                    MinPurchaseQuantity = table.Column<long>(type: "bigint", nullable: false),
                    Count = table.Column<long>(type: "bigint", nullable: false),
                    ImagesNames = table.Column<List<string>>(type: "text[]", nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DocumentsNames = table.Column<List<string>>(type: "text[]", nullable: true),
                    FarmId = table.Column<Guid>(type: "uuid", nullable: true),
                    SellerId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_Farms_FarmId",
                        column: x => x.FarmId,
                        principalTable: "Farms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Products_Sellers_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Sellers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Products_Subcategories_SubcategoryId",
                        column: x => x.SubcategoryId,
                        principalTable: "Subcategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Id",
                table: "Categories",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAddresses_Id",
                table: "CustomerAddresses",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPaymentData_Id",
                table: "CustomerPaymentData",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_AddressId",
                table: "Customers",
                column: "AddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Id",
                table: "Customers",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_PaymentDataId",
                table: "Customers",
                column: "PaymentDataId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DaysOfWeek_Id",
                table: "DaysOfWeek",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Farmers_AddressId",
                table: "Farmers",
                column: "AddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Farmers_Id",
                table: "Farmers",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Farmers_PaymentDataId",
                table: "Farmers",
                column: "PaymentDataId",
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
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Farms_PaymentDataId",
                table: "Farms",
                column: "PaymentDataId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Farms_ScheduleId",
                table: "Farms",
                column: "ScheduleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FarmsLogs_FarmId",
                table: "FarmsLogs",
                column: "FarmId");

            migrationBuilder.CreateIndex(
                name: "IX_FarmsLogs_Id",
                table: "FarmsLogs",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MonthesStatistics_BookedOrdersStatisticId",
                table: "MonthesStatistics",
                column: "BookedOrdersStatisticId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MonthesStatistics_CompletedOrdersStatisticId",
                table: "MonthesStatistics",
                column: "CompletedOrdersStatisticId",
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
                name: "IX_MonthesStatistics_NewOrdersStatisticId",
                table: "MonthesStatistics",
                column: "NewOrdersStatisticId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MonthesStatistics_ProcessingOrdersStatisticId",
                table: "MonthesStatistics",
                column: "ProcessingOrdersStatisticId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MonthesStatistics_SellerId",
                table: "MonthesStatistics",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_MonthesStatistics_TotalActivityStatisticId",
                table: "MonthesStatistics",
                column: "TotalActivityStatisticId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CreationDate",
                table: "Orders",
                column: "CreationDate",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DeliveryPointId",
                table: "Orders",
                column: "DeliveryPointId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Id",
                table: "Orders",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrdersItems_OrderId",
                table: "OrdersItems",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProducerAddresses_Id",
                table: "ProducerAddresses",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProducerPaymentData_Id",
                table: "ProducerPaymentData",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CreationDate",
                table: "Products",
                column: "CreationDate",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_Products_FarmId",
                table: "Products",
                column: "FarmId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Id",
                table: "Products",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_SellerId",
                table: "Products",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SubcategoryId",
                table: "Products",
                column: "SubcategoryId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Sellers_AddressId",
                table: "Sellers",
                column: "AddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sellers_Id",
                table: "Sellers",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sellers_PaymentDataId",
                table: "Sellers",
                column: "PaymentDataId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sellers_ScheduleId",
                table: "Sellers",
                column: "ScheduleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subcategories_CategoryId",
                table: "Subcategories",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Farmers");

            migrationBuilder.DropTable(
                name: "FarmsLogs");

            migrationBuilder.DropTable(
                name: "MonthesStatistics");

            migrationBuilder.DropTable(
                name: "OrdersItems");

            migrationBuilder.DropTable(
                name: "ProducerAddresses");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "CustomerPaymentData");

            migrationBuilder.DropTable(
                name: "OrderGroupStatistic");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Farms");

            migrationBuilder.DropTable(
                name: "Sellers");

            migrationBuilder.DropTable(
                name: "Subcategories");

            migrationBuilder.DropTable(
                name: "CustomerAddresses");

            migrationBuilder.DropTable(
                name: "ProducerPaymentData");

            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "DaysOfWeek");
        }
    }
}
