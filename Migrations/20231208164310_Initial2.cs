using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjektLAB.Migrations
{
    public partial class Initial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CarsCarId",
                table: "Categories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CarsCarId",
                table: "Categories",
                column: "CarsCarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Cars_CarsCarId",
                table: "Categories",
                column: "CarsCarId",
                principalTable: "Cars",
                principalColumn: "CarId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Cars_CarsCarId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_CarsCarId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CarsCarId",
                table: "Categories");
        }
    }
}
