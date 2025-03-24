using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ticketsio.Migrations
{
    /// <inheritdoc />
    public partial class TicketSeatsModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seats_Ticket_TicketId",
                table: "Seats");

            migrationBuilder.DropIndex(
                name: "IX_Seats_TicketId",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "SeatId",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "TicketId",
                table: "Seats");

            migrationBuilder.CreateTable(
                name: "TicketSeats",
                columns: table => new
                {
                    TicketId = table.Column<int>(type: "int", nullable: false),
                    SeatId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketSeats", x => new { x.TicketId, x.SeatId });
                    table.ForeignKey(
                        name: "FK_TicketSeats_Seats_SeatId",
                        column: x => x.SeatId,
                        principalTable: "Seats",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TicketSeats_Ticket_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Ticket",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TicketSeats_SeatId",
                table: "TicketSeats",
                column: "SeatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TicketSeats");

            migrationBuilder.AddColumn<string>(
                name: "SeatId",
                table: "Ticket",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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
    }
}
