using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameModule.Migrations
{
    /// <inheritdoc />
    public partial class InitV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "GameModule");

            migrationBuilder.CreateTable(
                name: "MapTiles",
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
                    table.PrimaryKey("PK_MapTiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tribes",
                schema: "GameModule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Localization_X = table.Column<int>(type: "int", nullable: false),
                    Localization_Y = table.Column<int>(type: "int", nullable: false),
                    Resources_FreshFood = table.Column<int>(type: "int", nullable: false),
                    Resources_Wood = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tribes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Humans",
                schema: "GameModule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TribeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Localization_X = table.Column<int>(type: "int", nullable: false),
                    Localization_Y = table.Column<int>(type: "int", nullable: false),
                    FoodLevelPercentage = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Humans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Humans_Tribes_TribeId",
                        column: x => x.TribeId,
                        principalSchema: "GameModule",
                        principalTable: "Tribes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "HumanTasks",
                schema: "GameModule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TribeId = table.Column<int>(type: "int", nullable: false),
                    HumanId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    From = table.Column<DateTime>(type: "datetime2", nullable: false),
                    To = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Localization_X = table.Column<int>(type: "int", nullable: true),
                    Localization_Y = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HumanTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HumanTasks_Humans_HumanId",
                        column: x => x.HumanId,
                        principalSchema: "GameModule",
                        principalTable: "Humans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HumanTasks_Tribes_TribeId",
                        column: x => x.TribeId,
                        principalSchema: "GameModule",
                        principalTable: "Tribes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HumanTaskOrders",
                schema: "GameModule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TribeId = table.Column<int>(type: "int", nullable: false),
                    Added = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Localization_X = table.Column<int>(type: "int", nullable: false),
                    Localization_Y = table.Column<int>(type: "int", nullable: false),
                    HumanTaskId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HumanTaskOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HumanTaskOrders_HumanTasks_HumanTaskId",
                        column: x => x.HumanTaskId,
                        principalSchema: "GameModule",
                        principalTable: "HumanTasks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HumanTaskOrders_Tribes_TribeId",
                        column: x => x.TribeId,
                        principalSchema: "GameModule",
                        principalTable: "Tribes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Humans_TribeId",
                schema: "GameModule",
                table: "Humans",
                column: "TribeId");

            migrationBuilder.CreateIndex(
                name: "IX_HumanTaskOrders_HumanTaskId",
                schema: "GameModule",
                table: "HumanTaskOrders",
                column: "HumanTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_HumanTaskOrders_TribeId",
                schema: "GameModule",
                table: "HumanTaskOrders",
                column: "TribeId");

            migrationBuilder.CreateIndex(
                name: "IX_HumanTasks_HumanId",
                schema: "GameModule",
                table: "HumanTasks",
                column: "HumanId");

            migrationBuilder.CreateIndex(
                name: "IX_HumanTasks_TribeId",
                schema: "GameModule",
                table: "HumanTasks",
                column: "TribeId");

            migrationBuilder.CreateIndex(
                name: "IX_TribeRelocations_TribeId",
                schema: "GameModule",
                table: "TribeRelocations",
                column: "TribeId",
                unique: true);

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
                name: "HumanTaskOrders",
                schema: "GameModule");

            migrationBuilder.DropTable(
                name: "MapTiles",
                schema: "GameModule");

            migrationBuilder.DropTable(
                name: "TribeRelocations",
                schema: "GameModule");

            migrationBuilder.DropTable(
                name: "TribeStructures",
                schema: "GameModule");

            migrationBuilder.DropTable(
                name: "HumanTasks",
                schema: "GameModule");

            migrationBuilder.DropTable(
                name: "Humans",
                schema: "GameModule");

            migrationBuilder.DropTable(
                name: "Tribes",
                schema: "GameModule");
        }
    }
}
