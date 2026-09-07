using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LookTwiice.Migrations
{
    /// <inheritdoc />
    public partial class AddPhotoImageMetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Photos",
                newName: "WebUrl");

            migrationBuilder.AddColumn<long>(
                name: "FileSize",
                table: "Photos",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "Photos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MimeType",
                table: "Photos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OriginalFileName",
                table: "Photos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OriginalUrl",
                table: "Photos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                table: "Photos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Width",
                table: "Photos",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileSize",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "MimeType",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "OriginalFileName",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "OriginalUrl",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "Photos");

            migrationBuilder.RenameColumn(
                name: "WebUrl",
                table: "Photos",
                newName: "ImageUrl");
        }
    }
}
