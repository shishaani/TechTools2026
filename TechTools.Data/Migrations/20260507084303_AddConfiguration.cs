using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TechTools.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "GPUs",
                columns: new[] { "Id", "Brand", "Description", "Model", "Picture", "Price" },
                values: new object[,]
                {
                    { 1, "NVIDIA", "High-end gaming GPU with excellent performance.", "GeForce RTX 3080", "", 1590.00m },
                    { 2, "AMD", "Powerful GPU with great value for gaming.", "Radeon RX 6800 XT", "", 699.00m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GPUs",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "GPUs",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
