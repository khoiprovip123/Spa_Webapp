using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addnewtreatment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TreatmentCards_Services_ServiceID",
                table: "TreatmentCards");

            migrationBuilder.DropIndex(
                name: "IX_TreatmentCards_ServiceID",
                table: "TreatmentCards");

            migrationBuilder.DropColumn(
                name: "ServiceID",
                table: "TreatmentCards");

            migrationBuilder.AddColumn<string>(
                name: "CreateBy",
                table: "TreatmentCards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "TreatmentCards",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "TreatmentCards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "TreatmentSessions",
                columns: table => new
                {
                    SessionID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionNumber = table.Column<int>(type: "int", nullable: false),
                    TreatmentID = table.Column<long>(type: "bigint", nullable: false),
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
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TreatmendSessionDetails",
                columns: table => new
                {
                    TreatmendDetailID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionID = table.Column<long>(type: "bigint", nullable: false),
                    ServiceID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreatmendSessionDetails", x => x.TreatmendDetailID);
                    table.ForeignKey(
                        name: "FK_TreatmendSessionDetails_TreatmentSessions_SessionID",
                        column: x => x.SessionID,
                        principalTable: "TreatmentSessions",
                        principalColumn: "SessionID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TreatmendSessionDetails_SessionID",
                table: "TreatmendSessionDetails",
                column: "SessionID");

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentSessions_TreatmentID",
                table: "TreatmentSessions",
                column: "TreatmentID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TreatmendSessionDetails");

            migrationBuilder.DropTable(
                name: "TreatmentSessions");

            migrationBuilder.DropColumn(
                name: "CreateBy",
                table: "TreatmentCards");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "TreatmentCards");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "TreatmentCards");

            migrationBuilder.AddColumn<long>(
                name: "ServiceID",
                table: "TreatmentCards",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentCards_ServiceID",
                table: "TreatmentCards",
                column: "ServiceID");

            migrationBuilder.AddForeignKey(
                name: "FK_TreatmentCards_Services_ServiceID",
                table: "TreatmentCards",
                column: "ServiceID",
                principalTable: "Services",
                principalColumn: "ServiceID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
