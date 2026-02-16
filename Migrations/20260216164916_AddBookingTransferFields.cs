using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheSeatLineApi.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingTransferFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OriginalUserId",
                table: "Bookings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransferNote",
                table: "Bookings",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TransferredAt",
                table: "Bookings",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginalUserId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "TransferNote",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "TransferredAt",
                table: "Bookings");
        }
    }
}
