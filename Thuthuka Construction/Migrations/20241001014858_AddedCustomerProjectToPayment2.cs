using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thuthuka_Construction.Migrations
{
    /// <inheritdoc />
    public partial class AddedCustomerProjectToPayment2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_AspNetUsers_CustomerId",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_customerProjects_CustomerProjectId",
                table: "Payment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payment",
                table: "Payment");

            migrationBuilder.RenameTable(
                name: "Payment",
                newName: "payments");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_CustomerProjectId",
                table: "payments",
                newName: "IX_payments_CustomerProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_CustomerId",
                table: "payments",
                newName: "IX_payments_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_payments",
                table: "payments",
                column: "PaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_payments_AspNetUsers_CustomerId",
                table: "payments",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_payments_customerProjects_CustomerProjectId",
                table: "payments",
                column: "CustomerProjectId",
                principalTable: "customerProjects",
                principalColumn: "CustomerProjectId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_payments_AspNetUsers_CustomerId",
                table: "payments");

            migrationBuilder.DropForeignKey(
                name: "FK_payments_customerProjects_CustomerProjectId",
                table: "payments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_payments",
                table: "payments");

            migrationBuilder.RenameTable(
                name: "payments",
                newName: "Payment");

            migrationBuilder.RenameIndex(
                name: "IX_payments_CustomerProjectId",
                table: "Payment",
                newName: "IX_Payment_CustomerProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_payments_CustomerId",
                table: "Payment",
                newName: "IX_Payment_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payment",
                table: "Payment",
                column: "PaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_AspNetUsers_CustomerId",
                table: "Payment",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_customerProjects_CustomerProjectId",
                table: "Payment",
                column: "CustomerProjectId",
                principalTable: "customerProjects",
                principalColumn: "CustomerProjectId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
