using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameModule.Migrations
{
    /// <inheritdoc />
    public partial class TribeRelocationFromToNotNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MapTileId",
                schema: "GameModule",
                table: "HumanUnitTaskOrders");

            migrationBuilder.RenameColumn(
                name: "MapTileId",
                schema: "GameModule",
                table: "HumanUnitTasks",
                newName: "Localization_Y");

            migrationBuilder.AlterColumn<DateTime>(
                name: "To",
                schema: "GameModule",
                table: "TribeRelocations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "From",
                schema: "GameModule",
                table: "TribeRelocations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Localization_X",
                schema: "GameModule",
                table: "HumanUnitTasks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Localization_X",
                schema: "GameModule",
                table: "HumanUnitTaskOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Localization_Y",
                schema: "GameModule",
                table: "HumanUnitTaskOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Localization_X",
                schema: "GameModule",
                table: "HumanUnitTasks");

            migrationBuilder.DropColumn(
                name: "Localization_X",
                schema: "GameModule",
                table: "HumanUnitTaskOrders");

            migrationBuilder.DropColumn(
                name: "Localization_Y",
                schema: "GameModule",
                table: "HumanUnitTaskOrders");

            migrationBuilder.RenameColumn(
                name: "Localization_Y",
                schema: "GameModule",
                table: "HumanUnitTasks",
                newName: "MapTileId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "To",
                schema: "GameModule",
                table: "TribeRelocations",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "From",
                schema: "GameModule",
                table: "TribeRelocations",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "MapTileId",
                schema: "GameModule",
                table: "HumanUnitTaskOrders",
                type: "int",
                nullable: true);
        }
    }
}
