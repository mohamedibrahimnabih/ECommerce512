using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce512.Migrations
{
    /// <inheritdoc />
    public partial class AddPropTrafficToProductModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Traffic",
                table: "Products",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Traffic",
                table: "Products");
        }
    }
}
