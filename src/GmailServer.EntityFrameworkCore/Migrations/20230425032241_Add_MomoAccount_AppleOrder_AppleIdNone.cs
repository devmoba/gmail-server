using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GmailServer.Migrations
{
    public partial class Add_MomoAccount_AppleOrder_AppleIdNone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppAppleIdNones",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: false),
                    Password = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: false),
                    Username = table.Column<string>(type: "varchar(256)", unicode: false, maxLength: 256, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TakenTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PurchaseNumber = table.Column<int>(type: "int", nullable: false),
                    TakenOutNumber = table.Column<int>(type: "int", nullable: false),
                    Ccv = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecretAnswer1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecretAnswer2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecretAnswer3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddPaymentCompleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    RemovePaymentStatus = table.Column<int>(type: "int", nullable: false),
                    RemoveTakenTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RemoveUpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppAppleIdNones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppAppleOrders",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    URLPayment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LinkStatus = table.Column<int>(type: "int", nullable: false),
                    AddPaymentStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LinkTakenTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LinkCompletedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AddPaymentTakenTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AddPaymentCompletedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MomoAccount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppleID = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppAppleOrders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppMomoAccounts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "varchar(256)", unicode: false, maxLength: 256, nullable: false),
                    Password = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: false),
                    Email = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UDid1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UDid2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthenticateToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SessionKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SessionKey2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SetupKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentLinkCount = table.Column<int>(type: "int", nullable: false),
                    TotalLinkCount = table.Column<int>(type: "int", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastTakenTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CustmArg1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustmArg2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustmArg3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InUseDevice = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppMomoAccounts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppAppleIdNones_Email",
                table: "AppAppleIdNones",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppAppleIdNones_Id",
                table: "AppAppleIdNones",
                column: "Id")
                .Annotation("SqlServer:Include", new[] { "Username", "Status", "AddPaymentCompleted", "RemovePaymentStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_AppAppleOrders_Id",
                table: "AppAppleOrders",
                column: "Id")
                .Annotation("SqlServer:Include", new[] { "OrderID", "URLPayment", "LinkStatus", "AddPaymentStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_AppMomoAccounts_Id",
                table: "AppMomoAccounts",
                column: "Id")
                .Annotation("SqlServer:Include", new[] { "Email", "Status", "CurrentLinkCount", "TotalLinkCount" });

            migrationBuilder.CreateIndex(
                name: "IX_AppMomoAccounts_Username",
                table: "AppMomoAccounts",
                column: "Username",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppAppleIdNones");

            migrationBuilder.DropTable(
                name: "AppAppleOrders");

            migrationBuilder.DropTable(
                name: "AppMomoAccounts");
        }
    }
}
