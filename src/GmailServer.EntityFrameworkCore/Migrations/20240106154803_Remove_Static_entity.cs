using Microsoft.EntityFrameworkCore.Migrations;

namespace GmailServer.Migrations
{
    public partial class Remove_Static_entity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
               name: "AppStatistics");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
