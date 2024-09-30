using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thuthuka_Construction.Migrations
{
    /// <inheritdoc />
    public partial class FixedQuatationResorces : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_quotations_quotationResources_QuotationResourceId",
                table: "quotations");

            migrationBuilder.DropIndex(
                name: "IX_quotations_QuotationResourceId",
                table: "quotations");

            migrationBuilder.DropColumn(
                name: "QuotationResourceId",
                table: "quotations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuotationResourceId",
                table: "quotations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_quotations_QuotationResourceId",
                table: "quotations",
                column: "QuotationResourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_quotations_quotationResources_QuotationResourceId",
                table: "quotations",
                column: "QuotationResourceId",
                principalTable: "quotationResources",
                principalColumn: "QuotationResourceId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
