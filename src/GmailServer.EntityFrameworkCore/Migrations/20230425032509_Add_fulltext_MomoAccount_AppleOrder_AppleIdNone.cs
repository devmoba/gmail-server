using Microsoft.EntityFrameworkCore.Migrations;

namespace GmailServer.Migrations
{
    public partial class Add_fulltext_MomoAccount_AppleOrder_AppleIdNone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
            sql: "CREATE FULLTEXT CATALOG ftCatalog_AppAppleOrders AS DEFAULT",
            suppressTransaction: true);

            migrationBuilder.Sql(
                sql: "CREATE FULLTEXT INDEX ON AppAppleOrders(OrderID, URLPayment, MomoAccount, AppleID) KEY INDEX PK_AppAppleOrders",
                suppressTransaction: true);

            migrationBuilder.Sql(
            sql: "CREATE FULLTEXT CATALOG ftCatalog_AppMomoAccounts AS DEFAULT",
            suppressTransaction: true);

            migrationBuilder.Sql(
                sql: "CREATE FULLTEXT INDEX ON AppMomoAccounts(Username, Email) KEY INDEX PK_AppMomoAccounts",
                suppressTransaction: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
            sql: "DROP FULLTEXT INDEX ON AppAppleOrders",
            suppressTransaction: true);

            migrationBuilder.Sql(
            sql: "DROP FULLTEXT CATALOG ftCatalog_AppAppleOrders",
            suppressTransaction: true);

            migrationBuilder.Sql(
            sql: "DROP FULLTEXT INDEX ON AppMomoAccounts",
            suppressTransaction: true);

            migrationBuilder.Sql(
            sql: "DROP FULLTEXT CATALOG ftCatalog_AppMomoAccounts",
            suppressTransaction: true);
        }
    }
}
