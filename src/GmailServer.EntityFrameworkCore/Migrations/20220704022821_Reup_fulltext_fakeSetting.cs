using Microsoft.EntityFrameworkCore.Migrations;

namespace GmailServer.Migrations
{
    public partial class Reup_fulltext_fakeSetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
           sql: "CREATE FULLTEXT CATALOG ftCatalog_AppFakeSettings AS DEFAULT",
           suppressTransaction: true);

            migrationBuilder.Sql(
                sql: "CREATE FULLTEXT INDEX ON AppFakeSettings(DeviceType, Version, FakeVersion) KEY INDEX PK_AppFakeSettings",
                suppressTransaction: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
          sql: "DROP FULLTEXT INDEX ON AppFakeSettings",
          suppressTransaction: true);

            migrationBuilder.Sql(
                sql: "DROP FULLTEXT CATALOG ftCatalog_AppFakeSettings",
                suppressTransaction: true);
        }
    }
}
