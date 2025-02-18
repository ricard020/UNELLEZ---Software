using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace REPOSITORY.SQLiteMigrations
{
    /// <inheritdoc />
    public partial class ChangeExercisesTemplateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExerciseName",
                table: "ExerciseTemplate");

            migrationBuilder.AddColumn<short>(
                name: "IsRestingExercise",
                table: "ExerciseTemplate",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRestingExercise",
                table: "ExerciseTemplate");

            migrationBuilder.AddColumn<string>(
                name: "ExerciseName",
                table: "ExerciseTemplate",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
