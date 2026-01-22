using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinema.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNewData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Ticket",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SeatType",
                table: "Ticket",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Session",
                keyColumn: "SessionId",
                keyValue: 1,
                column: "DateTime",
                value: new DateTime(2026, 1, 21, 18, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "Session",
                keyColumn: "SessionId",
                keyValue: 2,
                column: "DateTime",
                value: new DateTime(2026, 1, 21, 21, 30, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "Session",
                keyColumn: "SessionId",
                keyValue: 3,
                column: "DateTime",
                value: new DateTime(2026, 1, 21, 19, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "Ticket",
                keyColumn: "TicketId",
                keyValue: 1,
                columns: new[] { "IsAvailable", "Price", "SeatType" },
                values: new object[] { true, 600.00m, 2 });

            migrationBuilder.UpdateData(
                table: "Ticket",
                keyColumn: "TicketId",
                keyValue: 2,
                columns: new[] { "IsAvailable", "SeatType" },
                values: new object[] { false, 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "SeatType",
                table: "Ticket");

            migrationBuilder.UpdateData(
                table: "Session",
                keyColumn: "SessionId",
                keyValue: 1,
                column: "DateTime",
                value: new DateTime(2026, 1, 20, 18, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "Session",
                keyColumn: "SessionId",
                keyValue: 2,
                column: "DateTime",
                value: new DateTime(2026, 1, 20, 21, 30, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "Session",
                keyColumn: "SessionId",
                keyValue: 3,
                column: "DateTime",
                value: new DateTime(2026, 1, 20, 19, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "Ticket",
                keyColumn: "TicketId",
                keyValue: 1,
                column: "Price",
                value: 450.00m);
        }
    }
}
