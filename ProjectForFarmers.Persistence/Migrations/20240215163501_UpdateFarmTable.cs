using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectForFarmers.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFarmTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Farms_PaymantData_PaymentDataId",
                table: "Farms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymantData",
                table: "PaymantData");

            migrationBuilder.RenameTable(
                name: "PaymantData",
                newName: "PaymentData");

            migrationBuilder.RenameColumn(
                name: "ReceivingTypes",
                table: "Farms",
                newName: "ReceivingMethods");

            migrationBuilder.RenameIndex(
                name: "IX_PaymantData_Id",
                table: "PaymentData",
                newName: "IX_PaymentData_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentData",
                table: "PaymentData",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Farms_PaymentData_PaymentDataId",
                table: "Farms",
                column: "PaymentDataId",
                principalTable: "PaymentData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Farms_PaymentData_PaymentDataId",
                table: "Farms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentData",
                table: "PaymentData");

            migrationBuilder.RenameTable(
                name: "PaymentData",
                newName: "PaymantData");

            migrationBuilder.RenameColumn(
                name: "ReceivingMethods",
                table: "Farms",
                newName: "ReceivingTypes");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentData_Id",
                table: "PaymantData",
                newName: "IX_PaymantData_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymantData",
                table: "PaymantData",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Farms_PaymantData_PaymentDataId",
                table: "Farms",
                column: "PaymentDataId",
                principalTable: "PaymantData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
