using Microsoft.EntityFrameworkCore.Migrations;

namespace Taxi.DAL.Migrations
{
    public partial class UpdateCalls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Street",
                table: "Calls",
                newName: "StartStreet");

            migrationBuilder.RenameColumn(
                name: "HomeNumber",
                table: "Calls",
                newName: "StartHomeNumber");

            migrationBuilder.AddColumn<int>(
                name: "EndHomeNumber",
                table: "Calls",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "EndStreet",
                table: "Calls",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndHomeNumber",
                table: "Calls");

            migrationBuilder.DropColumn(
                name: "EndStreet",
                table: "Calls");

            migrationBuilder.RenameColumn(
                name: "StartStreet",
                table: "Calls",
                newName: "Street");

            migrationBuilder.RenameColumn(
                name: "StartHomeNumber",
                table: "Calls",
                newName: "HomeNumber");
        }
    }
}
