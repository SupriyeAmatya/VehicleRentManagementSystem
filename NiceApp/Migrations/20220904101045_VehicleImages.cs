using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NiceApp.Migrations
{
    public partial class VehicleImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Vehiclenumber",
                table: "VehicleImages",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleImages_Vehiclenumber",
                table: "VehicleImages",
                column: "Vehiclenumber");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleImages_Vehicles_Vehiclenumber",
                table: "VehicleImages",
                column: "Vehiclenumber",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehicleImages_Vehicles_Vehiclenumber",
                table: "VehicleImages");

            migrationBuilder.DropIndex(
                name: "IX_VehicleImages_Vehiclenumber",
                table: "VehicleImages");

            migrationBuilder.AlterColumn<string>(
                name: "Vehiclenumber",
                table: "VehicleImages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
