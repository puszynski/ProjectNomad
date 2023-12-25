using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameModule.Migrations
{
    /// <inheritdoc />
    public partial class TaskLocalizationNotNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HumanTaskOrders_HumanTasks_HumanTaskId",
                schema: "GameModule",
                table: "HumanTaskOrders");

            migrationBuilder.DropIndex(
                name: "IX_HumanTaskOrders_HumanTaskId",
                schema: "GameModule",
                table: "HumanTaskOrders");

            migrationBuilder.DropColumn(
                name: "HumanTaskId",
                schema: "GameModule",
                table: "HumanTaskOrders");

            migrationBuilder.AlterColumn<int>(
                name: "Localization_Y",
                schema: "GameModule",
                table: "HumanTasks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Localization_X",
                schema: "GameModule",
                table: "HumanTasks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Localization_Y",
                schema: "GameModule",
                table: "HumanTasks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Localization_X",
                schema: "GameModule",
                table: "HumanTasks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "HumanTaskId",
                schema: "GameModule",
                table: "HumanTaskOrders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HumanTaskOrders_HumanTaskId",
                schema: "GameModule",
                table: "HumanTaskOrders",
                column: "HumanTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_HumanTaskOrders_HumanTasks_HumanTaskId",
                schema: "GameModule",
                table: "HumanTaskOrders",
                column: "HumanTaskId",
                principalSchema: "GameModule",
                principalTable: "HumanTasks",
                principalColumn: "Id");
        }
    }
}
