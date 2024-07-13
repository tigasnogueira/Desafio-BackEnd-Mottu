using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BikeRentalSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIdentifier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Identifier",
                table: "Motorcycles");

            migrationBuilder.DropColumn(
                name: "Identifier",
                table: "Couriers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Identifier",
                table: "Motorcycles",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Identifier",
                table: "Couriers",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
