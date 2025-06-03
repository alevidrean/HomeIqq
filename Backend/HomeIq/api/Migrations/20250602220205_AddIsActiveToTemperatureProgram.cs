using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class AddIsActiveToTemperatureProgram : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TemperatureProgram",
                table: "TemperatureProgram");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "545064ca-91c2-4d97-aa39-8df124e7cfa1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "72034907-21f4-43a6-a54c-2703f4a1e3a5");

            migrationBuilder.DropColumn(
                name: "Room",
                table: "TemperatureLog");

            migrationBuilder.DropColumn(
                name: "DayType",
                table: "TemperatureProgram");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "TemperatureProgram");

            migrationBuilder.DropColumn(
                name: "Hysteresis",
                table: "TemperatureProgram");

            migrationBuilder.DropColumn(
                name: "Offset",
                table: "TemperatureProgram");

            migrationBuilder.DropColumn(
                name: "Reference",
                table: "TemperatureProgram");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "TemperatureProgram");

            migrationBuilder.RenameTable(
                name: "TemperatureProgram",
                newName: "TemperaturePrograms");

            migrationBuilder.RenameColumn(
                name: "SensorId",
                table: "TemperatureLog",
                newName: "Camera");

            migrationBuilder.RenameColumn(
                name: "Room",
                table: "TemperaturePrograms",
                newName: "Name");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "EventLog",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "TemperaturePrograms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TemperaturePrograms",
                table: "TemperaturePrograms",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "TemperatureIntervals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProgramId = table.Column<int>(type: "int", nullable: false),
                    Start = table.Column<TimeSpan>(type: "time", nullable: false),
                    End = table.Column<TimeSpan>(type: "time", nullable: false),
                    Temperature = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemperatureIntervals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemperatureIntervals_TemperaturePrograms_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "TemperaturePrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "60357b66-71bd-42de-b092-752122f5a372", null, "Admin", "ADMIN" },
                    { "ce633bb7-fd92-48d2-a019-99c0c48a15fe", null, "User", "USER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TemperatureIntervals_ProgramId",
                table: "TemperatureIntervals",
                column: "ProgramId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TemperatureIntervals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TemperaturePrograms",
                table: "TemperaturePrograms");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "60357b66-71bd-42de-b092-752122f5a372");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ce633bb7-fd92-48d2-a019-99c0c48a15fe");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "TemperaturePrograms");

            migrationBuilder.RenameTable(
                name: "TemperaturePrograms",
                newName: "TemperatureProgram");

            migrationBuilder.RenameColumn(
                name: "Camera",
                table: "TemperatureLog",
                newName: "SensorId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "TemperatureProgram",
                newName: "Room");

            migrationBuilder.AddColumn<string>(
                name: "Room",
                table: "TemperatureLog",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "EventLog",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "DayType",
                table: "TemperatureProgram",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "EndTime",
                table: "TemperatureProgram",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<double>(
                name: "Hysteresis",
                table: "TemperatureProgram",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Offset",
                table: "TemperatureProgram",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Reference",
                table: "TemperatureProgram",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "StartTime",
                table: "TemperatureProgram",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddPrimaryKey(
                name: "PK_TemperatureProgram",
                table: "TemperatureProgram",
                column: "Id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "545064ca-91c2-4d97-aa39-8df124e7cfa1", null, "Admin", "ADMIN" },
                    { "72034907-21f4-43a6-a54c-2703f4a1e3a5", null, "User", "USER" }
                });
        }
    }
}
