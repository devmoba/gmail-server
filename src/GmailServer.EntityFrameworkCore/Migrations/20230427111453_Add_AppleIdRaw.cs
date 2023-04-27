using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GmailServer.Migrations
{
    public partial class Add_AppleIdRaw : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppAppleIdRaws",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Password = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: false),
                    SecretAnswer1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecretAnswer2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecretAnswer3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppAppleIdRaws", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppAppleIdRaws_Id",
                table: "AppAppleIdRaws",
                column: "Id")
                .Annotation("SqlServer:Include", new[] { "Created" });

            migrationBuilder.CreateIndex(
                name: "IX_AppAppleIdRaws_Username",
                table: "AppAppleIdRaws",
                column: "Username",
                unique: true,
                filter: "[Username] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppAppleIdRaws");
        }
    }
}
