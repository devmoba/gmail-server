using Microsoft.EntityFrameworkCore.Migrations;

namespace GmailServer.Migrations
{
    public partial class Add_fulltext_GmailType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AppGmailTypes_Name",
                table: "AppGmailTypes",
                column: "Name",
                unique: true);

            migrationBuilder.Sql(
             sql: "CREATE FULLTEXT CATALOG ftCatalog_AppGmailTypes AS DEFAULT",
             suppressTransaction: true);

            migrationBuilder.Sql(
                sql: "CREATE FULLTEXT INDEX ON AppGmailTypes(Name, FakeVersion, Version, DeviceType, Country) KEY INDEX PK_AppGmailTypes",
                suppressTransaction: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppGmailTypes_Name",
                table: "AppGmailTypes");

            migrationBuilder.Sql(
            sql: "DROP FULLTEXT INDEX ON AppGmailTypes",
            suppressTransaction: true);

            migrationBuilder.Sql(
                sql: "DROP FULLTEXT CATALOG ftCatalog_AppGmailTypes",
                suppressTransaction: true);
        }
    }
}
