using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thuthuka_Construction.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseAdjustment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_projects_AspNetUsers_CustomerId",
                table: "projects");

            migrationBuilder.DropForeignKey(
                name: "FK_quatations_projects_ProjectId",
                table: "quatations");

            migrationBuilder.DropTable(
                name: "customerRequirements");

            migrationBuilder.DropIndex(
                name: "IX_projects_CustomerId",
                table: "projects");

            migrationBuilder.DropColumn(
                name: "DateIssued",
                table: "quatations");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "projects");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "quatations",
                newName: "CustomerProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_quatations_ProjectId",
                table: "quatations",
                newName: "IX_quatations_CustomerProjectId");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "quatations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Resources",
                table: "quatations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "quatations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "projects",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "projects",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "customerProjects",
                columns: table => new
                {
                    CustomerProjectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    QuatationId = table.Column<int>(type: "int", nullable: false),
                    SelectDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customerProjects", x => x.CustomerProjectId);
                    table.ForeignKey(
                        name: "FK_customerProjects_AspNetUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_customerProjects_quatations_QuatationId",
                        column: x => x.QuatationId,
                        principalTable: "quatations",
                        principalColumn: "QuatationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_customerProjects_CustomerId",
                table: "customerProjects",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_customerProjects_QuatationId",
                table: "customerProjects",
                column: "QuatationId");

            migrationBuilder.AddForeignKey(
                name: "FK_quatations_customerProjects_CustomerProjectId",
                table: "quatations",
                column: "CustomerProjectId",
                principalTable: "customerProjects",
                principalColumn: "CustomerProjectId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_quatations_customerProjects_CustomerProjectId",
                table: "quatations");

            migrationBuilder.DropTable(
                name: "customerProjects");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "quatations");

            migrationBuilder.DropColumn(
                name: "Resources",
                table: "quatations");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "quatations");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "projects");

            migrationBuilder.RenameColumn(
                name: "CustomerProjectId",
                table: "quatations",
                newName: "ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_quatations_CustomerProjectId",
                table: "quatations",
                newName: "IX_quatations_ProjectId");

            migrationBuilder.AddColumn<DateOnly>(
                name: "DateIssued",
                table: "quatations",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AlterColumn<DateOnly>(
                name: "StartDate",
                table: "projects",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "EndDate",
                table: "projects",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "projects",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "customerRequirements",
                columns: table => new
                {
                    RequirementsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customerRequirements", x => x.RequirementsId);
                    table.ForeignKey(
                        name: "FK_customerRequirements_AspNetUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_customerRequirements_projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_projects_CustomerId",
                table: "projects",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_customerRequirements_CustomerId",
                table: "customerRequirements",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_customerRequirements_ProjectId",
                table: "customerRequirements",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_projects_AspNetUsers_CustomerId",
                table: "projects",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_quatations_projects_ProjectId",
                table: "quatations",
                column: "ProjectId",
                principalTable: "projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
