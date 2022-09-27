using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GmailServer.Migrations
{
    public partial class Add_TakenTime_Updated_AppleId_GmailPremium : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TakenTime",
                table: "AppGmailPremiums",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated",
                table: "AppGmailPremiums",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "TakenTime",
                table: "AppAppleIds",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated",
                table: "AppAppleIds",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_AppGmailPremiums_Email",
                table: "AppGmailPremiums",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppGmailPremiums_Email",
                table: "AppGmailPremiums");

            migrationBuilder.DropColumn(
                name: "TakenTime",
                table: "AppGmailPremiums");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "AppGmailPremiums");

            migrationBuilder.DropColumn(
                name: "TakenTime",
                table: "AppAppleIds");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "AppAppleIds");
        }
    }
}
