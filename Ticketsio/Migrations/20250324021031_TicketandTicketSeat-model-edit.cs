using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ticketsio.Migrations
{
    /// <inheritdoc />
    public partial class TicketandTicketSeatmodeledit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketSeats",
                table: "TicketSeats");

            migrationBuilder.RenameColumn(
                name: "TransactionId",
                table: "Ticket",
                newName: "SessionId");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "TicketSeats",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "TicketSeats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "TicketSeats",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "PaymentStripeId",
                table: "Ticket",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketSeats",
                table: "TicketSeats",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TicketSeats_TicketId",
                table: "TicketSeats",
                column: "TicketId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketSeats",
                table: "TicketSeats");

            migrationBuilder.DropIndex(
                name: "IX_TicketSeats_TicketId",
                table: "TicketSeats");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TicketSeats");

            migrationBuilder.DropColumn(
                name: "Count",
                table: "TicketSeats");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "TicketSeats");

            migrationBuilder.DropColumn(
                name: "PaymentStripeId",
                table: "Ticket");

            migrationBuilder.RenameColumn(
                name: "SessionId",
                table: "Ticket",
                newName: "TransactionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketSeats",
                table: "TicketSeats",
                columns: new[] { "TicketId", "SeatId" });
        }
    }
}
