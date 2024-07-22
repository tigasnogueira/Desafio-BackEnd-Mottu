using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BikeRentalSystem.Infrastructure.Identity.Migrations
{
    /// <inheritdoc />
    public partial class NewMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cfeac1ed-df79-4cd3-ad08-5758e3eb9b70", "AQAAAAIAAYagAAAAECZn3wiwyqQiJ0pjiB5awKXXorYbQDyhLkVlqmzkmECDWXDVN5Me2KKHuGLWXkXK2g==", "d9fa0776-f9c7-476a-bed6-935ad327decc" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f03ddfe4-7c06-4d9e-8e1a-c6ca69ad8ded", "AQAAAAIAAYagAAAAEMjCNCIbwN8wNNgXvhcl2VWKnqBfAFIFgEzBrWvrgxsdis/Dsb0oBbLHdIIwjyFGjQ==", "5df8cf16-e551-43fd-8d23-9c275720ef34" });
        }
    }
}
