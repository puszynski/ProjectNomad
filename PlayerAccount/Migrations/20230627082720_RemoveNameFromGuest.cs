using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountModule.Migrations
{
    /// <inheritdoc />
    public partial class RemoveNameFromGuest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GuestAccount_Name",
                schema: "AccountModule",
                table: "Accounts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GuestAccount_Name",
                schema: "AccountModule",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
