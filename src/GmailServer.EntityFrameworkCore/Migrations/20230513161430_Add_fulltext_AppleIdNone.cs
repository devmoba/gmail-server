using Microsoft.EntityFrameworkCore.Migrations;

namespace GmailServer.Migrations
{
    public partial class Add_fulltext_AppleIdNone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
            sql: "CREATE FULLTEXT CATALOG ftCatalog_AppAppleIdNones AS DEFAULT",
            suppressTransaction: true);

            migrationBuilder.Sql(
                sql: "CREATE FULLTEXT INDEX ON AppAppleIdNones(Username, Email) KEY INDEX PK_AppAppleIdNones",
                suppressTransaction: true);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
          sql: "DROP FULLTEXT INDEX ON AppAppleIdNones",
          suppressTransaction: true);

            migrationBuilder.Sql(
            sql: "DROP FULLTEXT CATALOG ftCatalog_AppAppleIdNones",
            suppressTransaction: true);
        }
    }
}
