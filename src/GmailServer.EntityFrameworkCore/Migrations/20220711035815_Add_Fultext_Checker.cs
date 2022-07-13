using Microsoft.EntityFrameworkCore.Migrations;

namespace GmailServer.Migrations
{
    public partial class Add_Fultext_Checker : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
            sql: "CREATE FULLTEXT CATALOG ftCatalog_AppCheckers AS DEFAULT",
            suppressTransaction: true);

            migrationBuilder.Sql(
                sql: "CREATE FULLTEXT INDEX ON AppCheckers(CheckerIP) KEY INDEX PK_AppCheckers",
                suppressTransaction: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
            sql: "DROP FULLTEXT INDEX ON AppCheckers",
            suppressTransaction: true);

            migrationBuilder.Sql(
                sql: "DROP FULLTEXT CATALOG ftCatalog_AppCheckers",
                suppressTransaction: true);
        }
    }
}
