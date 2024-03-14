using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyProClip.Migrations
{
    /// <inheritdoc />
    public partial class UpdateClip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClipLink",
                table: "Messages");

            migrationBuilder.AddColumn<int>(
                name: "ClipId",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ClipId",
                table: "Messages",
                column: "ClipId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Clips_ClipId",
                table: "Messages",
                column: "ClipId",
                principalTable: "Clips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Clips_ClipId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ClipId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ClipId",
                table: "Messages");

            migrationBuilder.AddColumn<string>(
                name: "ClipLink",
                table: "Messages",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
