using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CourseWork.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class DataInitializing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedDate", "Name", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 6, 1, 16, 57, 6, 645, DateTimeKind.Utc).AddTicks(3331), "Electronics", null },
                    { 2, new DateTime(2024, 6, 1, 16, 57, 6, 645, DateTimeKind.Utc).AddTicks(3333), "Clothing", null }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "CreatedDate", "Description", "ImageLocalPath", "ImageUrl", "Name", "Price", "Rate", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 6, 1, 19, 57, 6, 645, DateTimeKind.Local).AddTicks(3437), "High-performance smartphone with advanced features", null, null, "Smartphone", 599.99m, 4.5, null },
                    { 2, 1, new DateTime(2024, 6, 1, 19, 57, 6, 645, DateTimeKind.Local).AddTicks(3448), "Lightweight laptop for productivity on the go", null, null, "Laptop", 1099.99m, 4.2000000000000002, null },
                    { 3, 2, new DateTime(2024, 6, 1, 19, 57, 6, 645, DateTimeKind.Local).AddTicks(3449), "Casual cotton T-shirt for everyday wear", null, null, "T-shirt", 19.99m, 4.0, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
