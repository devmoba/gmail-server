using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GmailServer.Migrations
{
    public partial class Add_IndexInclude_Gmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AppGmails_Id",
                table: "AppGmails",
                column: "Id")
                .Annotation("SqlServer:Include", new[] { "Status", "Created", "Updated", "LastCheck", "RecoveryEmail", "Country" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppGmails_Id",
                table: "AppGmails");
        }
    }
}
