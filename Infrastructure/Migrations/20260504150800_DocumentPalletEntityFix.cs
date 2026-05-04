using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DocumentPalletEntityFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_pallets_WmsDocument_WmsDocumentId1",
                table: "pallets");

            migrationBuilder.DropIndex(
                name: "IX_pallets_WmsDocumentId1",
                table: "pallets");

            migrationBuilder.DropColumn(
                name: "WmsDocumentId1",
                table: "pallets");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WmsDocumentId1",
                table: "pallets",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_pallets_WmsDocumentId1",
                table: "pallets",
                column: "WmsDocumentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_pallets_WmsDocument_WmsDocumentId1",
                table: "pallets",
                column: "WmsDocumentId1",
                principalTable: "WmsDocument",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
