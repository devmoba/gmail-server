using Microsoft.EntityFrameworkCore.Migrations;

namespace GmailServer.Migrations
{
    public partial class Add_index_email_AppleId_GmailResource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AppGmailResources_Email",
                table: "AppGmailResources",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppAppleIds_Email",
                table: "AppAppleIds",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppGmailResources_Email",
                table: "AppGmailResources");

            migrationBuilder.DropIndex(
                name: "IX_AppAppleIds_Email",
                table: "AppAppleIds");
        }
    }
}
