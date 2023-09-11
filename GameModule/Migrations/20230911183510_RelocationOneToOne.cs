using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameModule.Migrations
{
    /// <inheritdoc />
    public partial class RelocationOneToOne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TribeRelocations",
                schema: "GameModule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TribeId = table.Column<int>(type: "int", nullable: false),
                    From = table.Column<DateTime>(type: "datetime2", nullable: false),
                    To = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Start_X = table.Column<int>(type: "int", nullable: false),
                    Start_Y = table.Column<int>(type: "int", nullable: false),
                    Destiny_X = table.Column<int>(type: "int", nullable: false),
                    Destiny_Y = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TribeRelocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TribeRelocations_Tribes_TribeId",
                        column: x => x.TribeId,
                        principalSchema: "GameModule",
                        principalTable: "Tribes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TribeRelocations_TribeId",
                schema: "GameModule",
                table: "TribeRelocations",
                column: "TribeId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TribeRelocations",
                schema: "GameModule");
        }
    }
}
