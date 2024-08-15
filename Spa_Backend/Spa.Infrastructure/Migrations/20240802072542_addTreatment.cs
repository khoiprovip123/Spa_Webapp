using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addTreatment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsTreatment",
                table: "Services",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NumbersOfSessions",
                table: "Services",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "PriceByTreatment",
                table: "Services",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TimeApart",
                table: "Services",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<long>(
                name: "TreatmentID",
                table: "Appointments",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TreatmentCards",
                columns: table => new
                {
                    TreatmentID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TreatmentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerID = table.Column<long>(type: "bigint", nullable: false),
                    ServiceID = table.Column<long>(type: "bigint", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalSessions = table.Column<int>(type: "int", nullable: false),
                    TimeApart = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreatmentCards", x => x.TreatmentID);
                    table.ForeignKey(
                        name: "FK_TreatmentCards_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TreatmentCards_Services_ServiceID",
                        column: x => x.ServiceID,
                        principalTable: "Services",
                        principalColumn: "ServiceID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentCards_CustomerID",
                table: "TreatmentCards",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentCards_ServiceID",
                table: "TreatmentCards",
                column: "ServiceID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TreatmentCards");

            migrationBuilder.DropColumn(
                name: "IsTreatment",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "NumbersOfSessions",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "PriceByTreatment",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "TimeApart",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "TreatmentID",
                table: "Appointments");
        }
    }
}
