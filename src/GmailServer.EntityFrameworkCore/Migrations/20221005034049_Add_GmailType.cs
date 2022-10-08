using Microsoft.EntityFrameworkCore.Migrations;

namespace GmailServer.Migrations
{
    public partial class Add_GmailType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "GmailTypeId",
                table: "AppGmails",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AppGmailTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    DeviceType = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    FakeVersion = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Version = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppGmailTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppGmails_GmailTypeId",
                table: "AppGmails",
                column: "GmailTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppGmails_AppGmailTypes_GmailTypeId",
                table: "AppGmails",
                column: "GmailTypeId",
                principalTable: "AppGmailTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppGmails_AppGmailTypes_GmailTypeId",
                table: "AppGmails");

            migrationBuilder.DropTable(
                name: "AppGmailTypes");

            migrationBuilder.DropIndex(
                name: "IX_AppGmails_GmailTypeId",
                table: "AppGmails");

            migrationBuilder.DropColumn(
                name: "GmailTypeId",
                table: "AppGmails");
        }
    }
}
