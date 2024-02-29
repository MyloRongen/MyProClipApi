using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyProClip.Migrations
{
    /// <inheritdoc />
    public partial class UpdateClips : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Clips",
                newName: "VideoUrl");

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                table: "Clips",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                table: "Clips");

            migrationBuilder.RenameColumn(
                name: "VideoUrl",
                table: "Clips",
                newName: "Content");
        }
    }
}
