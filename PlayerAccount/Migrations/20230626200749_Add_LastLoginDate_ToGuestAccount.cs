using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountModule.Migrations
{
    /// <inheritdoc />
    public partial class Add_LastLoginDate_ToGuestAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "GuestAccount_LastLogin",
                schema: "AccountModule",
                table: "Accounts",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GuestAccount_LastLogin",
                schema: "AccountModule",
                table: "Accounts");
        }
    }
}
