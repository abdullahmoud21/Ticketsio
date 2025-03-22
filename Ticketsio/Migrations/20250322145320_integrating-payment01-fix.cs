using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ticketsio.Migrations
{
    /// <inheritdoc />
    public partial class integratingpayment01fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Seats_SeatId",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_Ticket_SeatId",
                table: "Ticket");

            migrationBuilder.AddColumn<int>(
                name: "TicketId",
                table: "Seats",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Seats_TicketId",
                table: "Seats",
                column: "TicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_Ticket_TicketId",
                table: "Seats",
                column: "TicketId",
                principalTable: "Ticket",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seats_Ticket_TicketId",
                table: "Seats");

            migrationBuilder.DropIndex(
                name: "IX_Seats_TicketId",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "TicketId",
                table: "Seats");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_SeatId",
                table: "Ticket",
                column: "SeatId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Seats_SeatId",
                table: "Ticket",
                column: "SeatId",
                principalTable: "Seats",
                principalColumn: "Id");
        }
    }
}
