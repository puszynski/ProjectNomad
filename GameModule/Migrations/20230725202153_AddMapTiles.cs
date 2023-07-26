using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameModule.Migrations
{
    /// <inheritdoc />
    public partial class AddMapTiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Maps",
                schema: "GameModule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Localization_X = table.Column<int>(type: "int", nullable: false),
                    Localization_Y = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Food_ActualPoints = table.Column<int>(type: "int", nullable: false),
                    Food_MaxLimitPoints = table.Column<int>(type: "int", nullable: false),
                    Wood_ActualPoints = table.Column<int>(type: "int", nullable: false),
                    Wood_MaxLimitPoints = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maps", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Maps",
                schema: "GameModule");
        }
    }
}
