using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace REPOSITORY.SQLiteMigrations
{
    /// <inheritdoc />
    public partial class SQLiteInitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Descrip = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    EnergyZones = table.Column<string>(type: "varchar(160)", maxLength: 160, nullable: false),
                    RPMMin = table.Column<int>(type: "INTEGER", nullable: false),
                    RPMMax = table.Column<int>(type: "INTEGER", nullable: false),
                    HandsPositions = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TrainerID = table.Column<int>(type: "INTEGER", nullable: false),
                    Descrip = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    DateC = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateI = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Duration = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Descrip = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    Password = table.Column<string>(type: "BLOB", nullable: false),
                    PIN = table.Column<string>(type: "BLOB", nullable: false),
                    Email = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    NumberPhone = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    DateC = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateR = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateV = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserType = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SessionExercises",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SessionID = table.Column<int>(type: "INTEGER", nullable: false),
                    ExerciseID = table.Column<int>(type: "INTEGER", nullable: false),
                    DescripMov = table.Column<string>(type: "TEXT", nullable: false),
                    EnergyZone = table.Column<string>(type: "varchar(160)", maxLength: 160, nullable: false),
                    HandsPosition = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false),
                    RPMMed = table.Column<int>(type: "INTEGER", nullable: false),
                    RPMFin = table.Column<int>(type: "INTEGER", nullable: false),
                    DurationMin = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionExercises", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SessionExercises_Sessions_SessionID",
                        column: x => x.SessionID,
                        principalTable: "Sessions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Exercises",
                columns: new[] { "ID", "Descrip", "EnergyZones", "HandsPositions", "RPMMax", "RPMMin" },
                values: new object[,]
                {
                    { 1, "Plano Sentado", "Recuperación,Fondo,Fuerza,Intervalos,Día de la Carrera", "1,2,2.5", 110, 80 },
                    { 2, "Plano de Pie / Correr", "Fondo,Intervalos", "2,2.5", 110, 80 },
                    { 3, "Saltos", "Fondo,Intervalos", "2,2.5", 110, 80 },
                    { 4, "Escalada Sentado", "Fondo,Fuerza,Intervalos,Día de la Carrera", "2,2.5", 80, 60 },
                    { 5, "Escalada de Pie", "Fondo,Fuerza,Intervalos,Día de la Carrera", "3", 80, 60 },
                    { 6, "Correr en Montaña", "Fuerza,Intervalos,Día de la Carrera", "2,2.5", 80, 60 },
                    { 7, "Saltos en Montaña", "Fuerza,Intervalos,Día de la Carrera", "2,2.5,3", 80, 60 },
                    { 8, "Sprints en Plano", "Intervalos,Día de la Carrera", "2,2.5,3", 110, 80 },
                    { 9, "Sprints en Montaña", "Intervalos,Día de la Carrera", "2,2.5,3", 110, 80 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_SessionExercises_SessionID",
                table: "SessionExercises",
                column: "SessionID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropTable(
                name: "SessionExercises");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Sessions");
        }
    }
}
