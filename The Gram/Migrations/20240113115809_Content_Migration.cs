using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheGram.Migrations
{
    /// <inheritdoc />
    public partial class ContentMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Contents_Id",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Contents_ContentId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Contents_Id",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Contents_Id",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Reactions_Contents_ContentId",
                table: "Reactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contents",
                table: "Contents");

            migrationBuilder.RenameTable(
                name: "Contents",
                newName: "Content");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Content",
                table: "Content",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Content_Id",
                table: "Comments",
                column: "Id",
                principalTable: "Content",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Content_ContentId",
                table: "Images",
                column: "ContentId",
                principalTable: "Content",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Content_Id",
                table: "Messages",
                column: "Id",
                principalTable: "Content",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Content_Id",
                table: "Posts",
                column: "Id",
                principalTable: "Content",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reactions_Content_ContentId",
                table: "Reactions",
                column: "ContentId",
                principalTable: "Content",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Content_Id",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Content_ContentId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Content_Id",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Content_Id",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Reactions_Content_ContentId",
                table: "Reactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Content",
                table: "Content");

            migrationBuilder.RenameTable(
                name: "Content",
                newName: "Contents");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contents",
                table: "Contents",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Contents_Id",
                table: "Comments",
                column: "Id",
                principalTable: "Contents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Contents_ContentId",
                table: "Images",
                column: "ContentId",
                principalTable: "Contents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Contents_Id",
                table: "Messages",
                column: "Id",
                principalTable: "Contents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Contents_Id",
                table: "Posts",
                column: "Id",
                principalTable: "Contents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reactions_Contents_ContentId",
                table: "Reactions",
                column: "ContentId",
                principalTable: "Contents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
