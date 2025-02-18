using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace REPOSITORY.Migrations
{
    /// <inheritdoc />
    public partial class ColumnLogoInCompanyDataTableSetNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Logo",
                table: "CompanyData",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateC", "DateR", "DateV" },
                values: new object[] { new DateTime(2024, 11, 11, 20, 28, 52, 451, DateTimeKind.Local).AddTicks(967), new DateTime(2024, 11, 11, 20, 28, 52, 451, DateTimeKind.Local).AddTicks(987), new DateTime(2024, 11, 11, 20, 28, 52, 451, DateTimeKind.Local).AddTicks(989) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Logo",
                table: "CompanyData",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateC", "DateR", "DateV" },
                values: new object[] { new DateTime(2024, 11, 5, 19, 1, 52, 884, DateTimeKind.Local).AddTicks(1515), new DateTime(2024, 11, 5, 19, 1, 52, 884, DateTimeKind.Local).AddTicks(1527), new DateTime(2024, 11, 5, 19, 1, 52, 884, DateTimeKind.Local).AddTicks(1528) });
        }
    }
}
