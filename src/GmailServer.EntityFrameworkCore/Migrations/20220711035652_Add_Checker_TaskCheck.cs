using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GmailServer.Migrations
{
    public partial class Add_Checker_TaskCheck : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppCheckers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CheckerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CheckerIP = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    FreeRam = table.Column<double>(type: "float", nullable: false),
                    TotalRam = table.Column<double>(type: "float", nullable: false),
                    UsingThread = table.Column<int>(type: "int", nullable: false),
                    MaxThread = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastCheck = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppCheckers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppTaskChecks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailChecks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TypeCheck = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckerId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppTaskChecks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppTaskChecks_AppCheckers_CheckerId",
                        column: x => x.CheckerId,
                        principalTable: "AppCheckers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppCheckers_CheckerId",
                table: "AppCheckers",
                column: "CheckerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppTaskChecks_CheckerId",
                table: "AppTaskChecks",
                column: "CheckerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppTaskChecks");

            migrationBuilder.DropTable(
                name: "AppCheckers");
        }
    }
}
