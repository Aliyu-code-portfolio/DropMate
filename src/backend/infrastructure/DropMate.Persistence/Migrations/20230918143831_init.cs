using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DropMate.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b876fadd-d1f3-49ab-a4bb-53cbf43fd8a6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c2ee3932-f872-4b73-9867-dcc677f2979f");

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "Packages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "85c54091-e764-44e0-978c-3f8d4f3bf8d0", "9cb76fc3-c06d-4c38-98cc-260227fb8594", "Admin", "ADMIN" },
                    { "f2bc9e8b-9b97-495f-9ea4-ebcfe8417797", "74bc5333-adab-49fc-911e-81ddbade8aac", "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "85c54091-e764-44e0-978c-3f8d4f3bf8d0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f2bc9e8b-9b97-495f-9ea4-ebcfe8417797");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "Packages");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "b876fadd-d1f3-49ab-a4bb-53cbf43fd8a6", "4363756e-aaaf-434f-9725-0974a2de5859", "User", "USER" },
                    { "c2ee3932-f872-4b73-9867-dcc677f2979f", "42fed2c6-31df-4085-b59d-c7f242ad09ae", "Admin", "ADMIN" }
                });
        }
    }
}
