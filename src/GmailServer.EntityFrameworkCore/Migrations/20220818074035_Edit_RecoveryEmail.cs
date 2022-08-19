using Microsoft.EntityFrameworkCore.Migrations;

namespace GmailServer.Migrations
{
    public partial class Edit_RecoveryEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Emails",
                table: "AppRecoveryEmails");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "AppRecoveryEmails",
                type: "varchar(128)",
                unicode: false,
                maxLength: 128,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "AppRecoveryEmails");

            migrationBuilder.AddColumn<string>(
                name: "Emails",
                table: "AppRecoveryEmails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
