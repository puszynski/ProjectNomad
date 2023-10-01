using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameModule.Migrations
{
    /// <inheritdoc />
    public partial class TribeStructureTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TribeStructures",
                schema: "GameModule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TribeId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    PowerAndDurability = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TribeStructures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TribeStructures_Tribes_TribeId",
                        column: x => x.TribeId,
                        principalSchema: "GameModule",
                        principalTable: "Tribes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TribeStructures_TribeId",
                schema: "GameModule",
                table: "TribeStructures",
                column: "TribeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TribeStructures",
                schema: "GameModule");
        }
    }
}
