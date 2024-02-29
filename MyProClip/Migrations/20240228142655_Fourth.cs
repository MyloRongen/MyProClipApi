using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyProClip.Migrations
{
    /// <inheritdoc />
    public partial class Fourth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clips_AspNetUsers_UserID",
                table: "Clips");

            migrationBuilder.DropForeignKey(
                name: "FK_Clips_Categories_CategoryID",
                table: "Clips");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Clips",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "CategoryID",
                table: "Clips",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Clips_UserID",
                table: "Clips",
                newName: "IX_Clips_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Clips_CategoryID",
                table: "Clips",
                newName: "IX_Clips_CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clips_AspNetUsers_UserId",
                table: "Clips",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Clips_Categories_CategoryId",
                table: "Clips",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clips_AspNetUsers_UserId",
                table: "Clips");

            migrationBuilder.DropForeignKey(
                name: "FK_Clips_Categories_CategoryId",
                table: "Clips");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Clips",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Clips",
                newName: "CategoryID");

            migrationBuilder.RenameIndex(
                name: "IX_Clips_UserId",
                table: "Clips",
                newName: "IX_Clips_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Clips_CategoryId",
                table: "Clips",
                newName: "IX_Clips_CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Clips_AspNetUsers_UserID",
                table: "Clips",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Clips_Categories_CategoryID",
                table: "Clips",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "Id");
        }
    }
}
