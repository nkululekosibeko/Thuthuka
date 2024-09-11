using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thuthuka_Construction.Migrations
{
    /// <inheritdoc />
    public partial class FKCorrectSetUp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_projects_AspNetUsers_UserId",
                table: "projects");

            migrationBuilder.DropForeignKey(
                name: "FK_quatations_AspNetUsers_UserId",
                table: "quatations");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "quatations",
                newName: "ForemanId");

            migrationBuilder.RenameIndex(
                name: "IX_quatations_UserId",
                table: "quatations",
                newName: "IX_quatations_ForemanId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "projects",
                newName: "ForemanId");

            migrationBuilder.RenameIndex(
                name: "IX_projects_UserId",
                table: "projects",
                newName: "IX_projects_ForemanId");

            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "projects",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_projects_CustomerId",
                table: "projects",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_projects_AspNetUsers_CustomerId",
                table: "projects",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_projects_AspNetUsers_ForemanId",
                table: "projects",
                column: "ForemanId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_quatations_AspNetUsers_ForemanId",
                table: "quatations",
                column: "ForemanId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_projects_AspNetUsers_CustomerId",
                table: "projects");

            migrationBuilder.DropForeignKey(
                name: "FK_projects_AspNetUsers_ForemanId",
                table: "projects");

            migrationBuilder.DropForeignKey(
                name: "FK_quatations_AspNetUsers_ForemanId",
                table: "quatations");

            migrationBuilder.DropIndex(
                name: "IX_projects_CustomerId",
                table: "projects");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "projects");

            migrationBuilder.RenameColumn(
                name: "ForemanId",
                table: "quatations",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_quatations_ForemanId",
                table: "quatations",
                newName: "IX_quatations_UserId");

            migrationBuilder.RenameColumn(
                name: "ForemanId",
                table: "projects",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_projects_ForemanId",
                table: "projects",
                newName: "IX_projects_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_projects_AspNetUsers_UserId",
                table: "projects",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_quatations_AspNetUsers_UserId",
                table: "quatations",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
