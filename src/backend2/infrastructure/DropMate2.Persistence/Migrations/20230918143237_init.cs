using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DropMate2.Persistence.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "InitializedPayments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "InitializedPayments");
        }
    }
}
