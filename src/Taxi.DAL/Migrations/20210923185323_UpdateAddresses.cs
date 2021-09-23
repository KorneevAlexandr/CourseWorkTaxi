using Microsoft.EntityFrameworkCore.Migrations;

namespace Taxi.DAL.Migrations
{
    public partial class UpdateAddresses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Calls_Addresses_EndAddressId",
                table: "Calls");

            migrationBuilder.DropForeignKey(
                name: "FK_Calls_Addresses_StartAddressId",
                table: "Calls");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Calls_EndAddressId",
                table: "Calls");

            migrationBuilder.DropIndex(
                name: "IX_Calls_StartAddressId",
                table: "Calls");

            migrationBuilder.DropColumn(
                name: "EndAddressId",
                table: "Calls");

            migrationBuilder.RenameColumn(
                name: "StartAddressId",
                table: "Calls",
                newName: "HomeNumber");

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "Calls",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Street",
                table: "Calls");

            migrationBuilder.RenameColumn(
                name: "HomeNumber",
                table: "Calls",
                newName: "StartAddressId");

            migrationBuilder.AddColumn<int>(
                name: "EndAddressId",
                table: "Calls",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    District = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    HomeNumber = table.Column<int>(type: "int", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Calls_EndAddressId",
                table: "Calls",
                column: "EndAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Calls_StartAddressId",
                table: "Calls",
                column: "StartAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Calls_Addresses_EndAddressId",
                table: "Calls",
                column: "EndAddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Calls_Addresses_StartAddressId",
                table: "Calls",
                column: "StartAddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
