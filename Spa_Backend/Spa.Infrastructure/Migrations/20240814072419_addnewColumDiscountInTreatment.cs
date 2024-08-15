using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addnewColumDiscountInTreatment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsDone",
                table: "TreatmentDetails",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<double>(
                name: "AmountDiscount",
                table: "TreatmentDetails",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KindofDiscount",
                table: "TreatmentDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "TreatmentDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TotalAmount",
                table: "TreatmentDetails",
                type: "float",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TreatmentCode",
                table: "TreatmentCards",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "BillCode",
                table: "Bill",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountDiscount",
                table: "TreatmentDetails");

            migrationBuilder.DropColumn(
                name: "KindofDiscount",
                table: "TreatmentDetails");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "TreatmentDetails");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "TreatmentDetails");

            migrationBuilder.DropColumn(
                name: "BillCode",
                table: "Bill");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDone",
                table: "TreatmentDetails",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TreatmentCode",
                table: "TreatmentCards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
