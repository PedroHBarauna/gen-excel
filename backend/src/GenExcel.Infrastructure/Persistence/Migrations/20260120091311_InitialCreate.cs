using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GenExcel.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalCapacity = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.EventId);
                    table.CheckConstraint("CHK_Status", "Status IN ('Active', 'Cancelled', 'Finished')");
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    TicketId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.TicketId);
                });

            migrationBuilder.CreateTable(
                name: "TicketEvents",
                columns: table => new
                {
                    TicketEventId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    TicketId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FeeRate = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Available = table.Column<int>(type: "int", nullable: false),
                    Sold = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    SaleStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SaleEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Available"),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketEvents", x => x.TicketEventId);
                    table.ForeignKey(
                        name: "FK_TicketEvents_Event_EventId",
                        column: x => x.EventId,
                        principalTable: "Event",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketEvents_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "TicketId");
                });

            migrationBuilder.CreateTable(
                name: "Sale",
                columns: table => new
                {
                    SaleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketEventId = table.Column<int>(type: "int", nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    PurchaserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PurchaseEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PurchaserCpf = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    SaleDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentForm = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sale", x => x.SaleId);
                    table.CheckConstraint("CHK_PaymentStatus", "PaymentStatus IN ('Pending', 'Approved', 'Cancelled')");
                    table.ForeignKey(
                        name: "FK_Sale_Event_EventId",
                        column: x => x.EventId,
                        principalTable: "Event",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sale_TicketEvents_TicketEventId",
                        column: x => x.TicketEventId,
                        principalTable: "TicketEvents",
                        principalColumn: "TicketEventId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Event_DateTime",
                table: "Event",
                column: "EventDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Status",
                table: "Event",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_EventId",
                table: "Sale",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_TicketId",
                table: "Sale",
                column: "TicketEventId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketEvents_EventId",
                table: "TicketEvents",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketEvents_EventId_TicketId",
                table: "TicketEvents",
                columns: new[] { "EventId", "TicketId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TicketEvents_Status",
                table: "TicketEvents",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TicketEvents_TicketId",
                table: "TicketEvents",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TicketType",
                table: "Tickets",
                column: "TicketType",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sale");

            migrationBuilder.DropTable(
                name: "TicketEvents");

            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.DropTable(
                name: "Tickets");
        }
    }
}
