using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thuthuka_Construction.Migrations
{
    /// <inheritdoc />
    public partial class AddStartDateAndStatusResonToCP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "StartDate",
                table: "customerProjects",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatusReason",
                table: "customerProjects",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "customerProjects");

            migrationBuilder.DropColumn(
                name: "StatusReason",
                table: "customerProjects");
        }
    }
}
