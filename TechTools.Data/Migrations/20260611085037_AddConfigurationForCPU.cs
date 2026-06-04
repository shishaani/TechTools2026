using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TechTools.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddConfigurationForCPU : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CPUs",
                columns: new[] { "Id", "Brand", "Description", "Model", "Picture", "Price" },
                values: new object[,]
                {
                    { 1, "Intel", "A 24‑core, 6.0 GHz high‑end CPU made for top‑tier gaming and heavy workloads.", "Intel Core i9 14900K", "", 489.00m },
                    { 2, "AMD", "The fastest gaming CPU with 3D V‑Cache and top FPS efficiency.", "Ryzen 7 7800X3D", "", 354.90m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CPUs",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CPUs",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
