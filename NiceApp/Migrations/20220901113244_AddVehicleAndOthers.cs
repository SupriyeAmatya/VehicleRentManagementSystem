using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NiceApp.Migrations
{
    public partial class AddVehicleAndOthers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RentVehicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehicleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlateNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LicenseNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RentedFromLocation = table.Column<int>(type: "int", nullable: false),
                    RentedToLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RentedFromTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RentedToTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReturnApproved = table.Column<bool>(type: "bit", nullable: false),
                    Bill = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fine = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentVehicles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StationName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CanRent = table.Column<bool>(type: "bit", nullable: false),
                    CanReturn = table.Column<bool>(type: "bit", nullable: false),
                    NumberOfVehicles = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Vehiclenumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehicleImage1 = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleImages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlateNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InitialRentPrice = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RentRatePerHr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Penalty = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Availability = table.Column<bool>(type: "bit", nullable: false),
                    VehicleType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehicleKind = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WhereStored = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tracker = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RentVehicles");

            migrationBuilder.DropTable(
                name: "Stations");

            migrationBuilder.DropTable(
                name: "VehicleImages");

            migrationBuilder.DropTable(
                name: "Vehicles");
        }
    }
}
