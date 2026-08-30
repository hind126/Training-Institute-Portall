using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalAssignmentBrief.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMarkEnrollment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Mark",
                table: "Enrollments",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mark",
                table: "Enrollments");
        }
    }
}
