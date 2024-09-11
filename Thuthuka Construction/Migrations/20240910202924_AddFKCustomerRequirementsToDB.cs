using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thuthuka_Construction.Migrations
{
    /// <inheritdoc />
    public partial class AddFKCustomerRequirementsToDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "customerRequirements",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_customerRequirements_CustomerId",
                table: "customerRequirements",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_customerRequirements_AspNetUsers_CustomerId",
                table: "customerRequirements",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_customerRequirements_AspNetUsers_CustomerId",
                table: "customerRequirements");

            migrationBuilder.DropIndex(
                name: "IX_customerRequirements_CustomerId",
                table: "customerRequirements");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "customerRequirements");
        }
    }
}
