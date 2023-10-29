using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameModule.Migrations
{
    /// <inheritdoc />
    public partial class HumanTaskAddColumnIsCompleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                schema: "GameModule",
                table: "HumanTasks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompleted",
                schema: "GameModule",
                table: "HumanTasks");
        }
    }
}
