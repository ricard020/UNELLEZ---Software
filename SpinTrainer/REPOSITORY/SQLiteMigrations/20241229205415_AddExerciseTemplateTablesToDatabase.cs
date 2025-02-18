using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace REPOSITORY.SQLiteMigrations
{
    /// <inheritdoc />
    public partial class AddExerciseTemplateTablesToDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExerciseTemplate",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserID = table.Column<int>(type: "INTEGER", nullable: false),
                    ExerciseID = table.Column<int>(type: "INTEGER", nullable: false),
                    ExerciseName = table.Column<string>(type: "TEXT", nullable: false),
                    TemplateName = table.Column<string>(type: "TEXT", nullable: false),
                    DescripMov = table.Column<string>(type: "TEXT", nullable: false),
                    HandsPosition = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false),
                    RPMMed = table.Column<int>(type: "INTEGER", nullable: false),
                    RPMFin = table.Column<int>(type: "INTEGER", nullable: false),
                    ResistancePercentage = table.Column<int>(type: "INTEGER", nullable: false),
                    DurationMin = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseTemplate", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExerciseTemplate");
        }
    }
}
