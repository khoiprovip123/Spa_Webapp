using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateTreatmentRemoveSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TreatmendSessionDetails");

            migrationBuilder.DropTable(
                name: "TreatmentSessions");

            migrationBuilder.DropColumn(
                name: "TotalSessions",
                table: "TreatmentCards");

            migrationBuilder.CreateTable(
                name: "TreatmentDetails",
                columns: table => new
                {
                    TreatmentDetailID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceID = table.Column<long>(type: "bigint", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    QuantityDone = table.Column<int>(type: "int", nullable: false),
                    TreatmentID = table.Column<long>(type: "bigint", nullable: false),
                    IsDone = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreatmentDetails", x => x.TreatmentDetailID);
                    table.ForeignKey(
                        name: "FK_TreatmentDetails_Services_ServiceID",
                        column: x => x.ServiceID,
                        principalTable: "Services",
                        principalColumn: "ServiceID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TreatmentDetails_TreatmentCards_TreatmentID",
                        column: x => x.TreatmentID,
                        principalTable: "TreatmentCards",
                        principalColumn: "TreatmentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChooseServiceTreatment",
                columns: table => new
                {
                    AppointmentID = table.Column<long>(type: "bigint", nullable: false),
                    TreatmentDetailID = table.Column<long>(type: "bigint", nullable: false),
                    ID = table.Column<int>(type: "int", nullable: false),
                    QualityChooses = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChooseServiceTreatment", x => new { x.TreatmentDetailID, x.AppointmentID });
                    table.ForeignKey(
                        name: "FK_ChooseServiceTreatment_Appointments_AppointmentID",
                        column: x => x.AppointmentID,
                        principalTable: "Appointments",
                        principalColumn: "AppointmentID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChooseServiceTreatment_TreatmentDetails_TreatmentDetailID",
                        column: x => x.TreatmentDetailID,
                        principalTable: "TreatmentDetails",
                        principalColumn: "TreatmentDetailID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChooseServiceTreatment_AppointmentID",
                table: "ChooseServiceTreatment",
                column: "AppointmentID");

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentDetails_ServiceID",
                table: "TreatmentDetails",
                column: "ServiceID");

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentDetails_TreatmentID",
                table: "TreatmentDetails",
                column: "TreatmentID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChooseServiceTreatment");

            migrationBuilder.DropTable(
                name: "TreatmentDetails");

            migrationBuilder.AddColumn<int>(
                name: "TotalSessions",
                table: "TreatmentCards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TreatmentSessions",
                columns: table => new
                {
                    SessionID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TreatmentID = table.Column<long>(type: "bigint", nullable: false),
                    SessionNumber = table.Column<int>(type: "int", nullable: false),
                    isDone = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreatmentSessions", x => x.SessionID);
                    table.ForeignKey(
                        name: "FK_TreatmentSessions_TreatmentCards_TreatmentID",
                        column: x => x.TreatmentID,
                        principalTable: "TreatmentCards",
                        principalColumn: "TreatmentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TreatmendSessionDetails",
                columns: table => new
                {
                    TreatmendDetailID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceID = table.Column<long>(type: "bigint", nullable: false),
                    SessionID = table.Column<long>(type: "bigint", nullable: false),
                    IsDone = table.Column<bool>(type: "bit", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreatmendSessionDetails", x => x.TreatmendDetailID);
                    table.ForeignKey(
                        name: "FK_TreatmendSessionDetails_Services_ServiceID",
                        column: x => x.ServiceID,
                        principalTable: "Services",
                        principalColumn: "ServiceID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TreatmendSessionDetails_TreatmentSessions_SessionID",
                        column: x => x.SessionID,
                        principalTable: "TreatmentSessions",
                        principalColumn: "SessionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TreatmendSessionDetails_ServiceID",
                table: "TreatmendSessionDetails",
                column: "ServiceID");

            migrationBuilder.CreateIndex(
                name: "IX_TreatmendSessionDetails_SessionID",
                table: "TreatmendSessionDetails",
                column: "SessionID");

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentSessions_TreatmentID",
                table: "TreatmentSessions",
                column: "TreatmentID");
        }
    }
}
