using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thuthuka_Construction.Migrations
{
    /// <inheritdoc />
    public partial class MakeQuatationFieldNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_customerProjects_quatations_QuatationId",
                table: "customerProjects");

            migrationBuilder.AlterColumn<int>(
                name: "QuatationId",
                table: "customerProjects",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_customerProjects_quatations_QuatationId",
                table: "customerProjects",
                column: "QuatationId",
                principalTable: "quatations",
                principalColumn: "QuatationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_customerProjects_quatations_QuatationId",
                table: "customerProjects");

            migrationBuilder.AlterColumn<int>(
                name: "QuatationId",
                table: "customerProjects",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_customerProjects_quatations_QuatationId",
                table: "customerProjects",
                column: "QuatationId",
                principalTable: "quatations",
                principalColumn: "QuatationId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
