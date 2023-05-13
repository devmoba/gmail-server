using Microsoft.EntityFrameworkCore.Migrations;

namespace GmailServer.Migrations
{
    public partial class Add_fulltext_AppleId_GmailResource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
            sql: "CREATE FULLTEXT CATALOG ftCatalog_AppAppleIds AS DEFAULT",
            suppressTransaction: true);

            migrationBuilder.Sql(
                sql: "CREATE FULLTEXT INDEX ON AppAppleIds(Username, Email) KEY INDEX PK_AppAppleIds",
                suppressTransaction: true);

            migrationBuilder.Sql(
           sql: "CREATE FULLTEXT CATALOG ftCatalog_AppGmailResources AS DEFAULT",
           suppressTransaction: true);

            migrationBuilder.Sql(
                sql: "CREATE FULLTEXT INDEX ON AppGmailResources(Username, Email, Country) KEY INDEX PK_AppGmailResources",
                suppressTransaction: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
            sql: "DROP FULLTEXT INDEX ON AppAppleIds",
            suppressTransaction: true);

            migrationBuilder.Sql(
            sql: "DROP FULLTEXT CATALOG ftCatalog_AppAppleIds",
            suppressTransaction: true);

            migrationBuilder.Sql(
            sql: "DROP FULLTEXT INDEX ON AppGmailResources",
            suppressTransaction: true);

            migrationBuilder.Sql(
            sql: "DROP FULLTEXT CATALOG ftCatalog_AppGmailResources",
            suppressTransaction: true);
        }
    }
}
