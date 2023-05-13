using Microsoft.EntityFrameworkCore.Migrations;

namespace GmailServer.Migrations
{
    public partial class Edit_fulltext_MomoAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
           sql: "DROP FULLTEXT INDEX ON AppMomoAccounts",
           suppressTransaction: true);

            migrationBuilder.Sql(
            sql: "DROP FULLTEXT CATALOG ftCatalog_AppMomoAccounts",
            suppressTransaction: true);

            migrationBuilder.Sql(
            sql: "CREATE FULLTEXT CATALOG ftCatalog_AppMomoAccounts AS DEFAULT",
            suppressTransaction: true);

            migrationBuilder.Sql(
                sql: "CREATE FULLTEXT INDEX ON AppMomoAccounts(UploadGroup, Username, Email) KEY INDEX PK_AppMomoAccounts",
                suppressTransaction: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
           sql: "DROP FULLTEXT INDEX ON AppMomoAccounts",
           suppressTransaction: true);

            migrationBuilder.Sql(
            sql: "DROP FULLTEXT CATALOG ftCatalog_AppMomoAccounts",
            suppressTransaction: true);
        }
    }
}
