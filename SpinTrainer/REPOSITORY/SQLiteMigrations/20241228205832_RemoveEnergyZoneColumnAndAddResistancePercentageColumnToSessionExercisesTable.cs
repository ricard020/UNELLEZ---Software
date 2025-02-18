using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace REPOSITORY.SQLiteMigrations
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
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
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
        }
    }
}
