using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PaperProcessor.Migrations
{
    /// <inheritdoc />
    public partial class AddCoreFactoryTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "ProductCartons",
                columns: table => new
                {
                    ProductCartonId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    LengthMm = table.Column<int>(type: "int", nullable: false),
                    WidthMm = table.Column<int>(type: "int", nullable: false),
                    HeightMm = table.Column<int>(type: "int", nullable: false),
                    Style = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCartons", x => x.ProductCartonId);
                });

            migrationBuilder.CreateTable(
                name: "ProductionStages",
                columns: table => new
                {
                    ProductionStageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionStages", x => x.ProductionStageId);
                });

            migrationBuilder.CreateTable(
                name: "WorkOrders",
                columns: table => new
                {
                    WorkOrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkOrderNo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    ProductCartonId = table.Column<int>(type: "int", nullable: false),
                    QuantityOrdered = table.Column<int>(type: "int", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UnitSellPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkOrders", x => x.WorkOrderId);
                    table.ForeignKey(
                        name: "FK_WorkOrders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkOrders_ProductCartons_ProductCartonId",
                        column: x => x.ProductCartonId,
                        principalTable: "ProductCartons",
                        principalColumn: "ProductCartonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ProductionStages",
                columns: new[] { "ProductionStageId", "Name", "SortOrder" },
                values: new object[,]
                {
                    { 1, "Cutting", 1 },
                    { 2, "Printing", 2 },
                    { 3, "Folding", 3 },
                    { 4, "Gluing", 4 },
                    { 5, "QC", 5 },
                    { 6, "Packing", 6 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_CustomerId",
                table: "WorkOrders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_ProductCartonId",
                table: "WorkOrders",
                column: "ProductCartonId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_WorkOrderNo",
                table: "WorkOrders",
                column: "WorkOrderNo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductionStages");

            migrationBuilder.DropTable(
                name: "WorkOrders");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "ProductCartons");
        }
    }
}
