using Microsoft.EntityFrameworkCore.Migrations;

namespace GmailServer.Migrations
{
    public partial class Add_fultext_Statistic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
           sql: "CREATE FULLTEXT CATALOG ftCatalog_AppStatistics AS DEFAULT",
           suppressTransaction: true);

            migrationBuilder.Sql(
                sql: "CREATE FULLTEXT INDEX ON AppStatistics(EntityName, Username, Arg1, Arg2, Arg3) KEY INDEX PK_AppStatistics",
                suppressTransaction: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
            sql: "DROP FULLTEXT INDEX ON AppStatistics",
            suppressTransaction: true);

            migrationBuilder.Sql(
            sql: "DROP FULLTEXT CATALOG ftCatalog_AppStatistics",
            suppressTransaction: true);
        }
    }
}
