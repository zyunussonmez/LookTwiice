using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LookTwiice.Migrations
{
    /// <inheritdoc />
    public partial class UpdateContact : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "ContactInquiries");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "ContactInquiries",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "ContactInquiries",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "ContactInquiries");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "ContactInquiries");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "ContactInquiries",
                type: "text",
                nullable: true);
        }
    }
}
