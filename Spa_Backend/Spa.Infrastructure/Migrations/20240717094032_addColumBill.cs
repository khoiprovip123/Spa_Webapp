using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addColumBill : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TotalPrice",
                table: "BillItem",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "AmountDiscount",
                table: "Bill",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KindofDiscount",
                table: "Bill",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Bill",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "BillItem");

            migrationBuilder.DropColumn(
                name: "AmountDiscount",
                table: "Bill");

            migrationBuilder.DropColumn(
                name: "KindofDiscount",
                table: "Bill");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Bill");
        }
    }
}
