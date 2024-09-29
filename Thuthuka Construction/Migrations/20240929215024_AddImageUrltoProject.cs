using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thuthuka_Construction.Migrations
{
    /// <inheritdoc />
    public partial class AddImageUrltoProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "projects");
        }
    }
}
