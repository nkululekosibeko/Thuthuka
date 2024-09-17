using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thuthuka_Construction.Migrations
{
    /// <inheritdoc />
    public partial class AddCutomerFkToCustomerProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "customerProjects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_customerProjects_ProjectId",
                table: "customerProjects",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_customerProjects_projects_ProjectId",
                table: "customerProjects",
                column: "ProjectId",
                principalTable: "projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_customerProjects_projects_ProjectId",
                table: "customerProjects");

            migrationBuilder.DropIndex(
                name: "IX_customerProjects_ProjectId",
                table: "customerProjects");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "customerProjects");
        }
    }
}
