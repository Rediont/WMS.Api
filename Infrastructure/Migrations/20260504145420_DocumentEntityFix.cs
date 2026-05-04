using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DocumentEntityFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WmsDocument_Contracts_ContractId1",
                table: "WmsDocument");

            migrationBuilder.DropIndex(
                name: "IX_WmsDocument_ContractId1",
                table: "WmsDocument");

            migrationBuilder.DropColumn(
                name: "ContractId1",
                table: "WmsDocument");

            migrationBuilder.AlterColumn<double>(
                name: "RequiredCapacity",
                table: "PalletTypes",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContractId1",
                table: "WmsDocument",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "RequiredCapacity",
                table: "PalletTypes",
                type: "integer",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.CreateIndex(
                name: "IX_WmsDocument_ContractId1",
                table: "WmsDocument",
                column: "ContractId1");

            migrationBuilder.AddForeignKey(
                name: "FK_WmsDocument_Contracts_ContractId1",
                table: "WmsDocument",
                column: "ContractId1",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
