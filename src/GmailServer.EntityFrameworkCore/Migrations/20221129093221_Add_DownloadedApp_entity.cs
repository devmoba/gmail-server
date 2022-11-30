using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GmailServer.Migrations
{
    public partial class Add_DownloadedApp_entity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppDownloadedApps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppleIdFK = table.Column<long>(type: "bigint", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppDownloadedApps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppDownloadedApps_AppAppleIds_AppleIdFK",
                        column: x => x.AppleIdFK,
                        principalTable: "AppAppleIds",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppDownloadedApps_AppleIdFK",
                table: "AppDownloadedApps",
                column: "AppleIdFK");

            migrationBuilder.Sql(
            sql: "CREATE FULLTEXT CATALOG ftCatalog_AppDownloadedApps AS DEFAULT",
            suppressTransaction: true);

            migrationBuilder.Sql(
                sql: "CREATE FULLTEXT INDEX ON AppDownloadedApps(AppId, ProductId) KEY INDEX PK_AppDownloadedApps",
                suppressTransaction: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppDownloadedApps");

            migrationBuilder.Sql(
            sql: "DROP FULLTEXT INDEX ON AppDownloadedApps",
            suppressTransaction: true);

            migrationBuilder.Sql(
                sql: "DROP FULLTEXT CATALOG ftCatalog_AppDownloadedApps",
                suppressTransaction: true);
        }
    }
}
