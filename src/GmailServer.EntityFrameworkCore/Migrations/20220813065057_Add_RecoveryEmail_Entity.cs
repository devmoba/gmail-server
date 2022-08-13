using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GmailServer.Migrations
{
    public partial class Add_RecoveryEmail_Entity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppRecoveryEmails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Emails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "varchar(256)", unicode: false, maxLength: 256, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRecoveryEmails", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppRecoveryEmails_Id",
                table: "AppRecoveryEmails",
                column: "Id")
                .Annotation("SqlServer:Include", new[] { "Username", "Status" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppRecoveryEmails");
        }
    }
}
