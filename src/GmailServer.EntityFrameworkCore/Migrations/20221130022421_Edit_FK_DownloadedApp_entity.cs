using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GmailServer.Migrations
{
    public partial class Edit_FK_DownloadedApp_entity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppDownloadedApps_AppAppleIds_AppleIdFK",
                table: "AppDownloadedApps");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "AppDownloadedApps",
                type: "varchar(128)",
                unicode: false,
                maxLength: 128,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppDownloadedApps_Id",
                table: "AppDownloadedApps",
                column: "Id")
                .Annotation("SqlServer:Include", new[] { "AppId", "ProductId", "Email", "Created" });

            migrationBuilder.AddForeignKey(
                name: "FK_AppDownloadedApps_AppAppleIds_AppleIdFK",
                table: "AppDownloadedApps",
                column: "AppleIdFK",
                principalTable: "AppAppleIds",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppDownloadedApps_AppAppleIds_AppleIdFK",
                table: "AppDownloadedApps");

            migrationBuilder.DropIndex(
                name: "IX_AppDownloadedApps_Id",
                table: "AppDownloadedApps");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "AppDownloadedApps");

            migrationBuilder.AddForeignKey(
                name: "FK_AppDownloadedApps_AppAppleIds_AppleIdFK",
                table: "AppDownloadedApps",
                column: "AppleIdFK",
                principalTable: "AppAppleIds",
                principalColumn: "Id");
        }
    }
}
