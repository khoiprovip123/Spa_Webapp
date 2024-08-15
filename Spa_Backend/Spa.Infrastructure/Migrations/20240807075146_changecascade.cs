using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changecascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TreatmendSessionDetails_TreatmentSessions_SessionID",
                table: "TreatmendSessionDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TreatmentSessions_TreatmentCards_TreatmentID",
                table: "TreatmentSessions");

            migrationBuilder.AddForeignKey(
                name: "FK_TreatmendSessionDetails_TreatmentSessions_SessionID",
                table: "TreatmendSessionDetails",
                column: "SessionID",
                principalTable: "TreatmentSessions",
                principalColumn: "SessionID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TreatmentSessions_TreatmentCards_TreatmentID",
                table: "TreatmentSessions",
                column: "TreatmentID",
                principalTable: "TreatmentCards",
                principalColumn: "TreatmentID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TreatmendSessionDetails_TreatmentSessions_SessionID",
                table: "TreatmendSessionDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TreatmentSessions_TreatmentCards_TreatmentID",
                table: "TreatmentSessions");

            migrationBuilder.AddForeignKey(
                name: "FK_TreatmendSessionDetails_TreatmentSessions_SessionID",
                table: "TreatmendSessionDetails",
                column: "SessionID",
                principalTable: "TreatmentSessions",
                principalColumn: "SessionID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TreatmentSessions_TreatmentCards_TreatmentID",
                table: "TreatmentSessions",
                column: "TreatmentID",
                principalTable: "TreatmentCards",
                principalColumn: "TreatmentID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
