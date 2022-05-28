using Microsoft.EntityFrameworkCore.Migrations;

namespace GmailServer.Migrations
{
    public partial class Addfulltextindexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
             sql: "CREATE FULLTEXT CATALOG ftCatalog_AppGmails AS DEFAULT",
             suppressTransaction: true);

            migrationBuilder.Sql(
                sql: "CREATE FULLTEXT INDEX ON AppGmails(Email, RecoveryEmail, Country) KEY INDEX PK_AppGmails",
                suppressTransaction: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
            sql: "DROP FULLTEXT INDEX ON AppGmails",
            suppressTransaction: true);

            migrationBuilder.Sql(
                sql: "DROP FULLTEXT CATALOG ftCatalog_AppGmails",
                suppressTransaction: true);
        }
    }
}
