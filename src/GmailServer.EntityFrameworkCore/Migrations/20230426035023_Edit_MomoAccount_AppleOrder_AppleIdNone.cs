using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GmailServer.Migrations
{
    public partial class Edit_MomoAccount_AppleOrder_AppleIdNone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppMomoAccounts_Id",
                table: "AppMomoAccounts");

            migrationBuilder.DropIndex(
                name: "IX_AppAppleOrders_Id",
                table: "AppAppleOrders");

            migrationBuilder.DropIndex(
                name: "IX_AppAppleIdNones_Id",
                table: "AppAppleIdNones");

            migrationBuilder.AddColumn<string>(
                name: "UploadGroup",
                table: "AppMomoAccounts",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_AppMomoAccounts_Id",
                table: "AppMomoAccounts",
                column: "Id")
                .Annotation("SqlServer:Include", new[] { "UploadGroup", "CreatedTime", "Email", "Status", "CurrentLinkCount", "TotalLinkCount", "LastUpdateTime", "LastTakenTime" });

            migrationBuilder.CreateIndex(
                name: "IX_AppAppleOrders_Id",
                table: "AppAppleOrders",
                column: "Id")
                .Annotation("SqlServer:Include", new[] { "OrderID", "URLPayment", "LinkStatus", "AddPaymentStatus", "LinkTakenTime", "LinkCompletedTime", "AddPaymentTakenTime", "AddPaymentCompletedTime" });

            migrationBuilder.CreateIndex(
                name: "IX_AppAppleIdNones_Id",
                table: "AppAppleIdNones",
                column: "Id")
                .Annotation("SqlServer:Include", new[] { "Username", "Created", "Status", "AddPaymentCompleted", "RemovePaymentStatus", "RemoveTakenTime", "RemoveUpdateTime" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppMomoAccounts_Id",
                table: "AppMomoAccounts");

            migrationBuilder.DropIndex(
                name: "IX_AppAppleOrders_Id",
                table: "AppAppleOrders");

            migrationBuilder.DropIndex(
                name: "IX_AppAppleIdNones_Id",
                table: "AppAppleIdNones");

            migrationBuilder.DropColumn(
                name: "UploadGroup",
                table: "AppMomoAccounts");

            migrationBuilder.CreateIndex(
                name: "IX_AppMomoAccounts_Id",
                table: "AppMomoAccounts",
                column: "Id")
                .Annotation("SqlServer:Include", new[] { "Email", "Status", "CurrentLinkCount", "TotalLinkCount" });

            migrationBuilder.CreateIndex(
                name: "IX_AppAppleOrders_Id",
                table: "AppAppleOrders",
                column: "Id")
                .Annotation("SqlServer:Include", new[] { "OrderID", "URLPayment", "LinkStatus", "AddPaymentStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_AppAppleIdNones_Id",
                table: "AppAppleIdNones",
                column: "Id")
                .Annotation("SqlServer:Include", new[] { "Username", "Status", "AddPaymentCompleted", "RemovePaymentStatus" });
        }
    }
}
