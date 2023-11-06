using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameModule.Migrations
{
    /// <inheritdoc />
    public partial class WorldZoneParametersHumanThermalLvl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ThermalLevelPercentage",
                schema: "GameModule",
                table: "Humans",
                type: "int",
                nullable: false,
                defaultValue: 50);

            migrationBuilder.CreateTable(
                name: "WorldZoneParameter",
                schema: "GameModule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Zone = table.Column<int>(type: "int", nullable: false),
                    AverageTemperature = table.Column<int>(type: "int", nullable: false),
                    IsWind = table.Column<bool>(type: "bit", nullable: false),
                    IsRain = table.Column<bool>(type: "bit", nullable: false),
                    IsSnow = table.Column<bool>(type: "bit", nullable: false),
                    IsBlizzard = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorldZoneParameter", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorldZoneParameter",
                schema: "GameModule");

            migrationBuilder.DropColumn(
                name: "ThermalLevelPercentage",
                schema: "GameModule",
                table: "Humans");
        }
    }
}
