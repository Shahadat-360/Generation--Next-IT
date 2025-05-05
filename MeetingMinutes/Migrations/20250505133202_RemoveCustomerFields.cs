using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeetingMinutes.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCustomerFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Individual_Customer_Tbl");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Individual_Customer_Tbl");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Individual_Customer_Tbl");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Corporate_Customer_Tbl");

            migrationBuilder.DropColumn(
                name: "ContactPerson",
                table: "Corporate_Customer_Tbl");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Corporate_Customer_Tbl");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Corporate_Customer_Tbl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Individual_Customer_Tbl",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Individual_Customer_Tbl",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Individual_Customer_Tbl",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Corporate_Customer_Tbl",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactPerson",
                table: "Corporate_Customer_Tbl",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Corporate_Customer_Tbl",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Corporate_Customer_Tbl",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
