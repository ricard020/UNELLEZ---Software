using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace REPOSITORY.Migrations
{
    /// <inheritdoc />
    public partial class ChangeExerciseTemplatesTable : Migration
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

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateC", "DateR", "DateV" },
                values: new object[] { new DateTime(2025, 1, 8, 22, 45, 29, 423, DateTimeKind.Local).AddTicks(4067), new DateTime(2025, 1, 8, 22, 45, 29, 426, DateTimeKind.Local).AddTicks(8064), new DateTime(2025, 1, 8, 22, 45, 29, 426, DateTimeKind.Local).AddTicks(8839) });
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
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateC", "DateR", "DateV" },
                values: new object[] { new DateTime(2024, 12, 29, 17, 17, 37, 5, DateTimeKind.Local).AddTicks(6070), new DateTime(2024, 12, 29, 17, 17, 37, 10, DateTimeKind.Local).AddTicks(3834), new DateTime(2024, 12, 29, 17, 17, 37, 10, DateTimeKind.Local).AddTicks(4601) });
        }
    }
}
