using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Cinema.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Movie",
                columns: new[] { "MovieId", "DurationMinutes", "Genre", "Title" },
                values: new object[,]
                {
                    { 1, 169, "Фантастика", "Интерстеллар" },
                    { 2, 175, "Криминал", "Крестный отец" },
                    { 3, 142, "Драма", "Побег из Шоушенка" }
                });

            migrationBuilder.InsertData(
                table: "Session",
                columns: new[] { "SessionId", "DateTime", "HallNumber", "MovieId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 20, 18, 0, 0, 0, DateTimeKind.Local), 1, 1 },
                    { 2, new DateTime(2026, 1, 20, 21, 30, 0, 0, DateTimeKind.Local), 1, 1 },
                    { 3, new DateTime(2026, 1, 20, 19, 0, 0, 0, DateTimeKind.Local), 2, 2 }
                });

            migrationBuilder.InsertData(
                table: "Ticket",
                columns: new[] { "TicketId", "Price", "RowNumber", "SeatNumber", "SessionId" },
                values: new object[,]
                {
                    { 1, 450.00m, 5, 10, 1 },
                    { 2, 450.00m, 5, 11, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Movie",
                keyColumn: "MovieId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Session",
                keyColumn: "SessionId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Session",
                keyColumn: "SessionId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Ticket",
                keyColumn: "TicketId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Ticket",
                keyColumn: "TicketId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Movie",
                keyColumn: "MovieId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Session",
                keyColumn: "SessionId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Movie",
                keyColumn: "MovieId",
                keyValue: 1);
        }
    }
}
