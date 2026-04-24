using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedDocumentStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_pallets_ContractReceipts_InboundReceiptId",
                table: "pallets");

            migrationBuilder.DropForeignKey(
                name: "FK_pallets_ContractReceipts_InboundReceiptId1",
                table: "pallets");

            migrationBuilder.DropForeignKey(
                name: "FK_pallets_ContractShipments_ShipmentId",
                table: "pallets");

            migrationBuilder.DropTable(
                name: "ContractReceipts");

            migrationBuilder.DropTable(
                name: "ContractShipments");

            migrationBuilder.DropIndex(
                name: "IX_pallets_ShipmentId",
                table: "pallets");

            migrationBuilder.DropColumn(
                name: "ShipmentId",
                table: "pallets");

            migrationBuilder.RenameColumn(
                name: "weight",
                table: "pallets",
                newName: "Weight");

            migrationBuilder.RenameColumn(
                name: "InboundReceiptId1",
                table: "pallets",
                newName: "WmsDocumentId1");

            migrationBuilder.RenameColumn(
                name: "InboundReceiptId",
                table: "pallets",
                newName: "WmsDocumentId");

            migrationBuilder.RenameIndex(
                name: "IX_pallets_InboundReceiptId1",
                table: "pallets",
                newName: "IX_pallets_WmsDocumentId1");

            migrationBuilder.RenameIndex(
                name: "IX_pallets_InboundReceiptId",
                table: "pallets",
                newName: "IX_pallets_WmsDocumentId");

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "Contracts",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "InventoryBalance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClientId = table.Column<int>(type: "integer", nullable: false),
                    ContractId = table.Column<int>(type: "integer", nullable: false),
                    PalletTypeId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryBalance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryBalance_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryBalance_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryBalance_PalletTypes_PalletTypeId",
                        column: x => x.PalletTypeId,
                        principalTable: "PalletTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WmsDocument",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DocumentType = table.Column<int>(type: "integer", nullable: false),
                    ContractId = table.Column<int>(type: "integer", nullable: false),
                    ContractId1 = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WmsDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WmsDocument_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WmsDocument_Contracts_ContractId1",
                        column: x => x.ContractId1,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WmsDocumentItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WmsDocumentId = table.Column<int>(type: "integer", nullable: false),
                    PalletTypeId = table.Column<int>(type: "integer", nullable: false),
                    ExpectedAmount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WmsDocumentItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WmsDocumentItems_PalletTypes_PalletTypeId",
                        column: x => x.PalletTypeId,
                        principalTable: "PalletTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WmsDocumentItems_WmsDocument_WmsDocumentId",
                        column: x => x.WmsDocumentId,
                        principalTable: "WmsDocument",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryBalance_ClientId_ContractId_PalletTypeId",
                table: "InventoryBalance",
                columns: new[] { "ClientId", "ContractId", "PalletTypeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryBalance_ContractId",
                table: "InventoryBalance",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryBalance_PalletTypeId",
                table: "InventoryBalance",
                column: "PalletTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WmsDocument_ContractId",
                table: "WmsDocument",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_WmsDocument_ContractId1",
                table: "WmsDocument",
                column: "ContractId1");

            migrationBuilder.CreateIndex(
                name: "IX_WmsDocumentItems_PalletTypeId",
                table: "WmsDocumentItems",
                column: "PalletTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WmsDocumentItems_WmsDocumentId",
                table: "WmsDocumentItems",
                column: "WmsDocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_pallets_WmsDocument_WmsDocumentId",
                table: "pallets",
                column: "WmsDocumentId",
                principalTable: "WmsDocument",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_pallets_WmsDocument_WmsDocumentId1",
                table: "pallets",
                column: "WmsDocumentId1",
                principalTable: "WmsDocument",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_pallets_WmsDocument_WmsDocumentId",
                table: "pallets");

            migrationBuilder.DropForeignKey(
                name: "FK_pallets_WmsDocument_WmsDocumentId1",
                table: "pallets");

            migrationBuilder.DropTable(
                name: "InventoryBalance");

            migrationBuilder.DropTable(
                name: "WmsDocumentItems");

            migrationBuilder.DropTable(
                name: "WmsDocument");

            migrationBuilder.RenameColumn(
                name: "Weight",
                table: "pallets",
                newName: "weight");

            migrationBuilder.RenameColumn(
                name: "WmsDocumentId1",
                table: "pallets",
                newName: "InboundReceiptId1");

            migrationBuilder.RenameColumn(
                name: "WmsDocumentId",
                table: "pallets",
                newName: "InboundReceiptId");

            migrationBuilder.RenameIndex(
                name: "IX_pallets_WmsDocumentId1",
                table: "pallets",
                newName: "IX_pallets_InboundReceiptId1");

            migrationBuilder.RenameIndex(
                name: "IX_pallets_WmsDocumentId",
                table: "pallets",
                newName: "IX_pallets_InboundReceiptId");

            migrationBuilder.AddColumn<int>(
                name: "ShipmentId",
                table: "pallets",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "Contracts",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateTable(
                name: "ContractReceipts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContractId1 = table.Column<int>(type: "integer", nullable: false),
                    PalletTypeId = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<int>(type: "integer", nullable: false),
                    ContractId = table.Column<int>(type: "integer", nullable: false),
                    ReceiptDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractReceipts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractReceipts_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContractReceipts_Contracts_ContractId1",
                        column: x => x.ContractId1,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContractReceipts_PalletTypes_PalletTypeId",
                        column: x => x.PalletTypeId,
                        principalTable: "PalletTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContractShipments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContractId1 = table.Column<int>(type: "integer", nullable: false),
                    ContractId = table.Column<int>(type: "integer", nullable: false),
                    ShipmentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractShipments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractShipments_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContractShipments_Contracts_ContractId1",
                        column: x => x.ContractId1,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_pallets_ShipmentId",
                table: "pallets",
                column: "ShipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractReceipts_ContractId",
                table: "ContractReceipts",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractReceipts_ContractId1",
                table: "ContractReceipts",
                column: "ContractId1");

            migrationBuilder.CreateIndex(
                name: "IX_ContractReceipts_PalletTypeId",
                table: "ContractReceipts",
                column: "PalletTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractShipments_ContractId",
                table: "ContractShipments",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractShipments_ContractId1",
                table: "ContractShipments",
                column: "ContractId1");

            migrationBuilder.AddForeignKey(
                name: "FK_pallets_ContractReceipts_InboundReceiptId",
                table: "pallets",
                column: "InboundReceiptId",
                principalTable: "ContractReceipts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_pallets_ContractReceipts_InboundReceiptId1",
                table: "pallets",
                column: "InboundReceiptId1",
                principalTable: "ContractReceipts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_pallets_ContractShipments_ShipmentId",
                table: "pallets",
                column: "ShipmentId",
                principalTable: "ContractShipments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
