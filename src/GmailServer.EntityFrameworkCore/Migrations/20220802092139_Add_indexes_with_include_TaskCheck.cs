using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GmailServer.Migrations
{
    public partial class Add_indexes_with_include_TaskCheck : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AppTaskChecks_Id",
                table: "AppTaskChecks",
                column: "Id")
                .Annotation("SqlServer:Include", new[] { "Username", "EmailChecks", "Status", "TypeCheck", "CheckerId", "Created" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppTaskChecks_Id",
                table: "AppTaskChecks");
        }
    }
}
