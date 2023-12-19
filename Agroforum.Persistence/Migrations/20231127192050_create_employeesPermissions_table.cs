using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agroforum.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class create_employeesPermissions_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeesPermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    FarmId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdateFarmInformation = table.Column<bool>(type: "boolean", nullable: false),
                    CreateProduct = table.Column<bool>(type: "boolean", nullable: false),
                    UpdateProduct = table.Column<bool>(type: "boolean", nullable: false),
                    DeleteProduct = table.Column<bool>(type: "boolean", nullable: false),
                    ListProductForSale = table.Column<bool>(type: "boolean", nullable: false),
                    WithdrawProductFromSale = table.Column<bool>(type: "boolean", nullable: false),
                    AddEmployee = table.Column<bool>(type: "boolean", nullable: false),
                    UpdateEmployeePermissions = table.Column<bool>(type: "boolean", nullable: false),
                    DeleteEmployee = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeesPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeesPermissions_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeesPermissions_Farms_FarmId",
                        column: x => x.FarmId,
                        principalTable: "Farms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeesPermissions_AccountId",
                table: "EmployeesPermissions",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeesPermissions_FarmId",
                table: "EmployeesPermissions",
                column: "FarmId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeesPermissions_Id",
                table: "EmployeesPermissions",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeesPermissions");
        }
    }
}
