using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addmqhservicewithdetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TreatmendSessionDetails_ServiceID",
                table: "TreatmendSessionDetails",
                column: "ServiceID");

            migrationBuilder.AddForeignKey(
                name: "FK_TreatmendSessionDetails_Services_ServiceID",
                table: "TreatmendSessionDetails",
                column: "ServiceID",
                principalTable: "Services",
                principalColumn: "ServiceID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TreatmendSessionDetails_Services_ServiceID",
                table: "TreatmendSessionDetails");

            migrationBuilder.DropIndex(
                name: "IX_TreatmendSessionDetails_ServiceID",
                table: "TreatmendSessionDetails");
        }
    }
}
