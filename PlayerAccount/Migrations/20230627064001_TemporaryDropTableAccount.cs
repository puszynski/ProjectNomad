using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountModule.Migrations
{
    /// <inheritdoc />
    public partial class TemporaryDropTableAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts",
                schema: "AccountModule");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "AccountModule");

            migrationBuilder.CreateTable(
                name: "Accounts",
                schema: "AccountModule",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GuestAccount_LastLoginDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GuestAccount_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GuestAccount_ReLoginToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegisteredAccount_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegisteredAccount_Password = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });
        }
    }
}
