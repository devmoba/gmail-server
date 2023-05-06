using Microsoft.EntityFrameworkCore.Migrations;

namespace GmailServer.Migrations
{
    public partial class Add_OwnerConfig_and_fulltext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppOwnerConfigs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppOwnerConfigs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppOwnerConfigs_Key",
                table: "AppOwnerConfigs",
                column: "Key",
                unique: true);

            migrationBuilder.Sql(
           sql: "CREATE FULLTEXT CATALOG ftCatalog_AppOwnerConfigs AS DEFAULT",
           suppressTransaction: true);

            migrationBuilder.Sql(
                sql: "CREATE FULLTEXT INDEX ON AppOwnerConfigs([Key]) KEY INDEX PK_AppOwnerConfigs",
                suppressTransaction: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppOwnerConfigs");

            migrationBuilder.Sql(
           sql: "DROP FULLTEXT INDEX ON AppOwnerConfigs",
           suppressTransaction: true);

            migrationBuilder.Sql(
            sql: "DROP FULLTEXT CATALOG ftCatalog_AppOwnerConfigs",
            suppressTransaction: true);
        }
    }
}
