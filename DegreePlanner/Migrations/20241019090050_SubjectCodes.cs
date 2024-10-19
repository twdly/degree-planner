using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DegreePlanner.Migrations
{
    /// <inheritdoc />
    public partial class SubjectCodes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubjectCode",
                table: "Subjects",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubjectCode",
                table: "Subjects");
        }
    }
}
