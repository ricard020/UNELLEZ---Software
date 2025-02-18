using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace REPOSITORY.Migrations
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
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    ExerciseID = table.Column<int>(type: "int", nullable: false),
                    ExerciseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TemplateName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescripMov = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HandsPosition = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false),
                    RPMMed = table.Column<int>(type: "int", nullable: false),
                    RPMFin = table.Column<int>(type: "int", nullable: false),
                    ResistancePercentage = table.Column<int>(type: "int", nullable: false),
                    DurationMin = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseTemplate", x => x.ID);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateC", "DateR", "DateV" },
                values: new object[] { new DateTime(2024, 12, 29, 17, 17, 37, 5, DateTimeKind.Local).AddTicks(6070), new DateTime(2024, 12, 29, 17, 17, 37, 10, DateTimeKind.Local).AddTicks(3834), new DateTime(2024, 12, 29, 17, 17, 37, 10, DateTimeKind.Local).AddTicks(4601) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExerciseTemplate");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateC", "DateR", "DateV" },
                values: new object[] { new DateTime(2024, 12, 28, 16, 36, 7, 217, DateTimeKind.Local).AddTicks(9227), new DateTime(2024, 12, 28, 16, 36, 7, 219, DateTimeKind.Local).AddTicks(9415), new DateTime(2024, 12, 28, 16, 36, 7, 219, DateTimeKind.Local).AddTicks(9867) });
        }
    }
}
