using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce512.Migrations
{
    /// <inheritdoc />
    public partial class EditApplicationUserOTPModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationData",
                table: "ApplicationUserOTPs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ReleaseData",
                table: "ApplicationUserOTPs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpirationData",
                table: "ApplicationUserOTPs");

            migrationBuilder.DropColumn(
                name: "ReleaseData",
                table: "ApplicationUserOTPs");
        }
    }
}
