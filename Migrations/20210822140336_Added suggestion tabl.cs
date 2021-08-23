using Microsoft.EntityFrameworkCore.Migrations;

namespace Searchify.Migrations
{
    public partial class Addedsuggestiontabl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Suggestions",
                columns: table => new
                {
                    query = table.Column<string>(type: "text", nullable: true),
                    rank = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Suggestions");
        }
    }
}
