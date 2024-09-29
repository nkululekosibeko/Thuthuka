using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thuthuka_Construction.Migrations
{
    /// <inheritdoc />
    public partial class ChangedImprovedDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "quatations");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "projects");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "projects");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "projects");

            migrationBuilder.RenameColumn(
                name: "SelectDate",
                table: "customerProjects",
                newName: "CreatedDate");

            migrationBuilder.CreateTable(
                name: "resources",
                columns: table => new
                {
                    ResourceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PricePerUnit = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_resources", x => x.ResourceId);
                });

            migrationBuilder.CreateTable(
                name: "quotationResources",
                columns: table => new
                {
                    QuotationResourceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuotationId = table.Column<int>(type: "int", nullable: false),
                    ResourceId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quotationResources", x => x.QuotationResourceId);
                    table.ForeignKey(
                        name: "FK_quotationResources_resources_ResourceId",
                        column: x => x.ResourceId,
                        principalTable: "resources",
                        principalColumn: "ResourceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "quotations",
                columns: table => new
                {
                    QuotationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateOnly>(type: "date", nullable: false),
                    TotalCost = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerProjectId = table.Column<int>(type: "int", nullable: false),
                    QuotationResourceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quotations", x => x.QuotationId);
                    table.ForeignKey(
                        name: "FK_quotations_customerProjects_CustomerProjectId",
                        column: x => x.CustomerProjectId,
                        principalTable: "customerProjects",
                        principalColumn: "CustomerProjectId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_quotations_quotationResources_QuotationResourceId",
                        column: x => x.QuotationResourceId,
                        principalTable: "quotationResources",
                        principalColumn: "QuotationResourceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_quotationResources_QuotationId",
                table: "quotationResources",
                column: "QuotationId");

            migrationBuilder.CreateIndex(
                name: "IX_quotationResources_ResourceId",
                table: "quotationResources",
                column: "ResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_quotations_CustomerProjectId",
                table: "quotations",
                column: "CustomerProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_quotations_QuotationResourceId",
                table: "quotations",
                column: "QuotationResourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_quotationResources_quotations_QuotationId",
                table: "quotationResources",
                column: "QuotationId",
                principalTable: "quotations",
                principalColumn: "QuotationId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_quotationResources_quotations_QuotationId",
                table: "quotationResources");

            migrationBuilder.DropTable(
                name: "quotations");

            migrationBuilder.DropTable(
                name: "quotationResources");

            migrationBuilder.DropTable(
                name: "resources");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "customerProjects",
                newName: "SelectDate");

            migrationBuilder.AddColumn<DateOnly>(
                name: "EndDate",
                table: "projects",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "StartDate",
                table: "projects",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "quatations",
                columns: table => new
                {
                    QuatationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerProjectId = table.Column<int>(type: "int", nullable: false),
                    ForemanId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DateCreated = table.Column<DateOnly>(type: "date", nullable: false),
                    Resources = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalCost = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quatations", x => x.QuatationId);
                    table.ForeignKey(
                        name: "FK_quatations_AspNetUsers_ForemanId",
                        column: x => x.ForemanId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_quatations_customerProjects_CustomerProjectId",
                        column: x => x.CustomerProjectId,
                        principalTable: "customerProjects",
                        principalColumn: "CustomerProjectId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_quatations_CustomerProjectId",
                table: "quatations",
                column: "CustomerProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_quatations_ForemanId",
                table: "quatations",
                column: "ForemanId");
        }
    }
}
