using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SerenApp.Web.Migrations
{
    public partial class AddedDeviceReports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeviceReports",
                columns: table => new
                {
                    ID = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    BodyTemperatureAvg = table.Column<double>(type: "float", nullable: false),
                    BloodPressureAvg = table.Column<double>(type: "float", nullable: false),
                    BloodOxygenAvg = table.Column<double>(type: "float", nullable: false),
                    BatteryAvg = table.Column<double>(type: "float", nullable: false),
                    HeartFrequencyAvg = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceReports", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeviceReports");
        }
    }
}
