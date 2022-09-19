using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GmailServer.Migrations
{
    public partial class Add_GmailPremium_AppleId_entity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppAppleIds",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: false),
                    Password = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: false),
                    Username = table.Column<string>(type: "varchar(256)", unicode: false, maxLength: 256, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppAppleIds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppGmailPremiums",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: false),
                    Password = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: false),
                    Username = table.Column<string>(type: "varchar(256)", unicode: false, maxLength: 256, nullable: false),
                    RecoveryEmail = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppGmailPremiums", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppAppleIds_Id",
                table: "AppAppleIds",
                column: "Id")
                .Annotation("SqlServer:Include", new[] { "Username", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_AppGmailPremiums_Id",
                table: "AppGmailPremiums",
                column: "Id")
                .Annotation("SqlServer:Include", new[] { "Username", "Status" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppAppleIds");

            migrationBuilder.DropTable(
                name: "AppGmailPremiums");
        }
    }
}
