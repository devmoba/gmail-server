using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GmailServer.Migrations
{
    public partial class Add_Statistic_Entity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppStatistics",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Total = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    HashCode = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Arg1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Arg2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Arg3 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppStatistics", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppStatistics_HashCode",
                table: "AppStatistics",
                column: "HashCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppStatistics_Id",
                table: "AppStatistics",
                column: "Id")
                .Annotation("SqlServer:Include", new[] { "Date", "EntityName", "Type" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppStatistics");
        }
    }
}
