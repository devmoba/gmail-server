using Microsoft.EntityFrameworkCore.Migrations;

namespace GmailServer.Migrations
{
    public partial class Add_SecretAnswer_Dob_AppleId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DateOfBirth",
                table: "AppAppleIds",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecretAnswer1",
                table: "AppAppleIds",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecretAnswer2",
                table: "AppAppleIds",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecretAnswer3",
                table: "AppAppleIds",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "AppAppleIds");

            migrationBuilder.DropColumn(
                name: "SecretAnswer1",
                table: "AppAppleIds");

            migrationBuilder.DropColumn(
                name: "SecretAnswer2",
                table: "AppAppleIds");

            migrationBuilder.DropColumn(
                name: "SecretAnswer3",
                table: "AppAppleIds");
        }
    }
}
