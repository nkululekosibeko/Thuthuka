using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thuthuka_Construction.Migrations
{
    /// <inheritdoc />
    public partial class AddProgessTableToDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "progresses",
                columns: table => new
                {
                    ProgressId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerProjectId = table.Column<int>(type: "int", nullable: false),
                    CurrentPhase = table.Column<int>(type: "int", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_progresses", x => x.ProgressId);
                    table.ForeignKey(
                        name: "FK_progresses_customerProjects_CustomerProjectId",
                        column: x => x.CustomerProjectId,
                        principalTable: "customerProjects",
                        principalColumn: "CustomerProjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_progresses_CustomerProjectId",
                table: "progresses",
                column: "CustomerProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "progresses");
        }
    }
}
