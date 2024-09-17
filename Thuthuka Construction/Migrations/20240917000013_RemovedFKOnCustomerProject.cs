using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thuthuka_Construction.Migrations
{
    /// <inheritdoc />
    public partial class RemovedFKOnCustomerProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_customerProjects_quatations_QuatationId",
                table: "customerProjects");

            migrationBuilder.DropIndex(
                name: "IX_customerProjects_QuatationId",
                table: "customerProjects");

            migrationBuilder.DropColumn(
                name: "QuatationId",
                table: "customerProjects");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuatationId",
                table: "customerProjects",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_customerProjects_QuatationId",
                table: "customerProjects",
                column: "QuatationId");

            migrationBuilder.AddForeignKey(
                name: "FK_customerProjects_quatations_QuatationId",
                table: "customerProjects",
                column: "QuatationId",
                principalTable: "quatations",
                principalColumn: "QuatationId");
        }
    }
}
