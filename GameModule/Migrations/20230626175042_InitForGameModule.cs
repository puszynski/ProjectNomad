using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameModule.Migrations
{
    /// <inheritdoc />
    public partial class InitForGameModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "GameModule");

            migrationBuilder.CreateTable(
                name: "Tribes",
                schema: "GameModule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<int>(type: "int", nullable: false),
                    Localization_X = table.Column<int>(type: "int", nullable: false),
                    Localization_Y = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tribes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HumanUnits",
                schema: "GameModule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TribeId = table.Column<int>(type: "int", nullable: false),
                    Localization_X = table.Column<int>(type: "int", nullable: false),
                    Localization_Y = table.Column<int>(type: "int", nullable: false),
                    FoodLevelPercentage = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HumanUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HumanUnits_Tribes_TribeId",
                        column: x => x.TribeId,
                        principalSchema: "GameModule",
                        principalTable: "Tribes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HumanUnits_TribeId",
                schema: "GameModule",
                table: "HumanUnits",
                column: "TribeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HumanUnits",
                schema: "GameModule");

            migrationBuilder.DropTable(
                name: "Tribes",
                schema: "GameModule");
        }
    }
}
