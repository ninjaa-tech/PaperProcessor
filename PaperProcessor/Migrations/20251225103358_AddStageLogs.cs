using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaperProcessor.Migrations
{
    /// <inheritdoc />
    public partial class AddStageLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StageLogs",
                columns: table => new
                {
                    StageLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkOrderId = table.Column<int>(type: "int", nullable: false),
                    ProductionStageId = table.Column<int>(type: "int", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    QtyOut = table.Column<int>(type: "int", nullable: false),
                    ScrapQty = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StageLogs", x => x.StageLogId);
                    table.ForeignKey(
                        name: "FK_StageLogs_ProductionStages_ProductionStageId",
                        column: x => x.ProductionStageId,
                        principalTable: "ProductionStages",
                        principalColumn: "ProductionStageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StageLogs_WorkOrders_WorkOrderId",
                        column: x => x.WorkOrderId,
                        principalTable: "WorkOrders",
                        principalColumn: "WorkOrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StageLogs_ProductionStageId",
                table: "StageLogs",
                column: "ProductionStageId");

            migrationBuilder.CreateIndex(
                name: "IX_StageLogs_WorkOrderId_ProductionStageId",
                table: "StageLogs",
                columns: new[] { "WorkOrderId", "ProductionStageId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StageLogs");
        }
    }
}
