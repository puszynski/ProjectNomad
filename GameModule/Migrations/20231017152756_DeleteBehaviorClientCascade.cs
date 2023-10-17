using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameModule.Migrations
{
    /// <inheritdoc />
    public partial class DeleteBehaviorClientCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HumanTasks_Tribes_TribeId",
                schema: "GameModule",
                table: "HumanTasks");

            migrationBuilder.AddForeignKey(
                name: "FK_HumanTasks_Tribes_TribeId",
                schema: "GameModule",
                table: "HumanTasks",
                column: "TribeId",
                principalSchema: "GameModule",
                principalTable: "Tribes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HumanTasks_Tribes_TribeId",
                schema: "GameModule",
                table: "HumanTasks");

            migrationBuilder.AddForeignKey(
                name: "FK_HumanTasks_Tribes_TribeId",
                schema: "GameModule",
                table: "HumanTasks",
                column: "TribeId",
                principalSchema: "GameModule",
                principalTable: "Tribes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
