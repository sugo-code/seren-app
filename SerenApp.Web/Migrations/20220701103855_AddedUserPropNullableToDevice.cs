using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SerenApp.Web.Migrations
{
    public partial class AddedUserPropNullableToDevice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Users_UserID",
                table: "Devices");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserID",
                table: "Devices",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Users_UserID",
                table: "Devices",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Users_UserID",
                table: "Devices");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserID",
                table: "Devices",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Users_UserID",
                table: "Devices",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
