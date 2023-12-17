using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameModule.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIsComplitedFromHumanTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HumanTasks_HumanId",
                schema: "GameModule",
                table: "HumanTasks");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                schema: "GameModule",
                table: "HumanTasks");

            migrationBuilder.CreateIndex(
                name: "IX_HumanTasks_HumanId",
                schema: "GameModule",
                table: "HumanTasks",
                column: "HumanId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HumanTasks_HumanId",
                schema: "GameModule",
                table: "HumanTasks");

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                schema: "GameModule",
                table: "HumanTasks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_HumanTasks_HumanId",
                schema: "GameModule",
                table: "HumanTasks",
                column: "HumanId");
        }
    }
}
