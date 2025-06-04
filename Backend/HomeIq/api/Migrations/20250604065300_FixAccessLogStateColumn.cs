using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class FixAccessLogStateColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "60357b66-71bd-42de-b092-752122f5a372");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ce633bb7-fd92-48d2-a019-99c0c48a15fe");

            migrationBuilder.RenameColumn(
                name: "Direction",
                table: "AccessLog",
                newName: "LockState");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3d31d258-19f4-4e2e-b755-06a6931a68dd", null, "User", "USER" },
                    { "7fa1f324-c3c9-47a2-bfc8-4fb2177c6c8a", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3d31d258-19f4-4e2e-b755-06a6931a68dd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7fa1f324-c3c9-47a2-bfc8-4fb2177c6c8a");

            migrationBuilder.RenameColumn(
                name: "LockState",
                table: "AccessLog",
                newName: "Direction");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "60357b66-71bd-42de-b092-752122f5a372", null, "Admin", "ADMIN" },
                    { "ce633bb7-fd92-48d2-a019-99c0c48a15fe", null, "User", "USER" }
                });
        }
    }
}
