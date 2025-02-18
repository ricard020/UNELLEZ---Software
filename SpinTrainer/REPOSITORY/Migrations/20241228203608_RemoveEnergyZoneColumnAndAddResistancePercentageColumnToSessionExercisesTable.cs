using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace REPOSITORY.Migrations
{
    /// <inheritdoc />
    public partial class RemoveEnergyZoneColumnAndAddResistancePercentageColumnToSessionExercisesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnergyZone",
                table: "SessionExercises");

            migrationBuilder.DropColumn(
                name: "EnergyZones",
                table: "Exercises");

            migrationBuilder.AddColumn<int>(
                name: "ResistancePercentage",
                table: "SessionExercises",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateC", "DateR", "DateV" },
                values: new object[] { new DateTime(2024, 12, 28, 16, 36, 7, 217, DateTimeKind.Local).AddTicks(9227), new DateTime(2024, 12, 28, 16, 36, 7, 219, DateTimeKind.Local).AddTicks(9415), new DateTime(2024, 12, 28, 16, 36, 7, 219, DateTimeKind.Local).AddTicks(9867) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResistancePercentage",
                table: "SessionExercises");

            migrationBuilder.AddColumn<string>(
                name: "EnergyZone",
                table: "SessionExercises",
                type: "varchar(160)",
                maxLength: 160,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EnergyZones",
                table: "Exercises",
                type: "varchar(160)",
                maxLength: 160,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "ID",
                keyValue: 1,
                column: "EnergyZones",
                value: "Recuperación,Fondo,Fuerza,Intervalos,Día de la Carrera");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "ID",
                keyValue: 2,
                column: "EnergyZones",
                value: "Fondo,Intervalos");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "ID",
                keyValue: 3,
                column: "EnergyZones",
                value: "Fondo,Intervalos");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "ID",
                keyValue: 4,
                column: "EnergyZones",
                value: "Fondo,Fuerza,Intervalos,Día de la Carrera");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "ID",
                keyValue: 5,
                column: "EnergyZones",
                value: "Fondo,Fuerza,Intervalos,Día de la Carrera");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "ID",
                keyValue: 6,
                column: "EnergyZones",
                value: "Fuerza,Intervalos,Día de la Carrera");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "ID",
                keyValue: 7,
                column: "EnergyZones",
                value: "Fuerza,Intervalos,Día de la Carrera");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "ID",
                keyValue: 8,
                column: "EnergyZones",
                value: "Intervalos,Día de la Carrera");

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "ID",
                keyValue: 9,
                column: "EnergyZones",
                value: "Intervalos,Día de la Carrera");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateC", "DateR", "DateV" },
                values: new object[] { new DateTime(2024, 11, 11, 20, 28, 52, 451, DateTimeKind.Local).AddTicks(967), new DateTime(2024, 11, 11, 20, 28, 52, 451, DateTimeKind.Local).AddTicks(987), new DateTime(2024, 11, 11, 20, 28, 52, 451, DateTimeKind.Local).AddTicks(989) });
        }
    }
}
