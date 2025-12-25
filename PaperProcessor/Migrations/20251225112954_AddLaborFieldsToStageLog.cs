using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaperProcessor.Migrations
{
    /// <inheritdoc />
    public partial class AddLaborFieldsToStageLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "HourlyRateSnapshot",
                table: "StageLogs",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "LaborMinutes",
                table: "StageLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HourlyRateSnapshot",
                table: "StageLogs");

            migrationBuilder.DropColumn(
                name: "LaborMinutes",
                table: "StageLogs");
        }
    }
}
