using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LookTwiice.Migrations
{
    /// <inheritdoc />
    public partial class ProfileImgRmv : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImageUrl",
                table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfileImageUrl",
                table: "AspNetUsers",
                type: "text",
                nullable: true);
        }
    }
}
