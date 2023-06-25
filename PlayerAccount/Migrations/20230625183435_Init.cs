using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountModule.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
