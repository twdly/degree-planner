using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DegreePlanner.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePrerequisites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prerequisites");

            migrationBuilder.CreateTable(
                name: "SubjectSubject",
                columns: table => new
                {
                    PrerequisiteForSubjectId = table.Column<int>(type: "int", nullable: false),
                    PrerequisitesSubjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectSubject", x => new { x.PrerequisiteForSubjectId, x.PrerequisitesSubjectId });
                    table.ForeignKey(
                        name: "FK_SubjectSubject_Subjects_PrerequisiteForSubjectId",
                        column: x => x.PrerequisiteForSubjectId,
                        principalTable: "Subjects",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectSubject_Subjects_PrerequisitesSubjectId",
                        column: x => x.PrerequisitesSubjectId,
                        principalTable: "Subjects",
                        principalColumn: "SubjectId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubjectSubject_PrerequisitesSubjectId",
                table: "SubjectSubject",
                column: "PrerequisitesSubjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubjectSubject");

            migrationBuilder.CreateTable(
                name: "Prerequisites",
                columns: table => new
                {
                    PrerequisiteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prerequisites", x => x.PrerequisiteId);
                    table.ForeignKey(
                        name: "FK_Prerequisites_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "SubjectId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prerequisites_SubjectId",
                table: "Prerequisites",
                column: "SubjectId");
        }
    }
}
