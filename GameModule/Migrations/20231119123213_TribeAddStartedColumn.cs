using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameModule.Migrations
{
    /// <inheritdoc />
    public partial class TribeAddStartedColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Started",
                schema: "GameModule",
                table: "Tribes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Started",
                schema: "GameModule",
                table: "Tribes");
        }
    }
}
