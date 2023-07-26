using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameModule.Migrations
{
    /// <inheritdoc />
    public partial class RenameNameToMapTiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Maps",
                schema: "GameModule",
                table: "Maps");

            migrationBuilder.RenameTable(
                name: "Maps",
                schema: "GameModule",
                newName: "MapTiles",
                newSchema: "GameModule");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MapTiles",
                schema: "GameModule",
                table: "MapTiles",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MapTiles",
                schema: "GameModule",
                table: "MapTiles");

            migrationBuilder.RenameTable(
                name: "MapTiles",
                schema: "GameModule",
                newName: "Maps",
                newSchema: "GameModule");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Maps",
                schema: "GameModule",
                table: "Maps",
                column: "Id");
        }
    }
}
