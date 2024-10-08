using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DegreePlanner.Migrations
{
    /// <inheritdoc />
    public partial class SubjectCredits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrerequisiteSubject");

            migrationBuilder.DropColumn(
                name: "Credits",
                table: "Subjects");

            migrationBuilder.RenameColumn(
                name: "Credits",
                table: "Degrees",
                newName: "ElectiveCredits");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Users",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "10000, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "CoreCredits",
                table: "Degrees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Prerequisites_SubjectId",
                table: "Prerequisites",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prerequisites_Subjects_SubjectId",
                table: "Prerequisites",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "SubjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prerequisites_Subjects_SubjectId",
                table: "Prerequisites");

            migrationBuilder.DropIndex(
                name: "IX_Prerequisites_SubjectId",
                table: "Prerequisites");

            migrationBuilder.DropColumn(
                name: "CoreCredits",
                table: "Degrees");

            migrationBuilder.RenameColumn(
                name: "ElectiveCredits",
                table: "Degrees",
                newName: "Credits");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Users",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "10000, 1");

            migrationBuilder.AddColumn<int>(
                name: "Credits",
                table: "Subjects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PrerequisiteSubject",
                columns: table => new
                {
                    PrerequisitesPrerequisiteId = table.Column<int>(type: "int", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrerequisiteSubject", x => new { x.PrerequisitesPrerequisiteId, x.SubjectId });
                    table.ForeignKey(
                        name: "FK_PrerequisiteSubject_Prerequisites_PrerequisitesPrerequisiteId",
                        column: x => x.PrerequisitesPrerequisiteId,
                        principalTable: "Prerequisites",
                        principalColumn: "PrerequisiteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrerequisiteSubject_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrerequisiteSubject_SubjectId",
                table: "PrerequisiteSubject",
                column: "SubjectId");
        }
    }
}
