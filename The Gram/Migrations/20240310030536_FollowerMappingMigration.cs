using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheGram.Migrations
{
    /// <inheritdoc />
    public partial class FollowerMappingMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfileFollowerMapping_UserProfiles_FollowerId",
                table: "ProfileFollowerMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileFollowerMapping_UserProfiles_ProfileId",
                table: "ProfileFollowerMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileFriendMapping_UserProfiles_FriendId",
                table: "ProfileFriendMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileFriendMapping_UserProfiles_ProfileId",
                table: "ProfileFriendMapping");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProfileFriendMapping",
                table: "ProfileFriendMapping");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProfileFollowerMapping",
                table: "ProfileFollowerMapping");

            migrationBuilder.RenameTable(
                name: "ProfileFriendMapping",
                newName: "ProfileFriendMappings");

            migrationBuilder.RenameTable(
                name: "ProfileFollowerMapping",
                newName: "ProfileFollowerMappings");

            migrationBuilder.RenameIndex(
                name: "IX_ProfileFriendMapping_ProfileId",
                table: "ProfileFriendMappings",
                newName: "IX_ProfileFriendMappings_ProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_ProfileFriendMapping_FriendId",
                table: "ProfileFriendMappings",
                newName: "IX_ProfileFriendMappings_FriendId");

            migrationBuilder.RenameIndex(
                name: "IX_ProfileFollowerMapping_ProfileId",
                table: "ProfileFollowerMappings",
                newName: "IX_ProfileFollowerMappings_ProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_ProfileFollowerMapping_FollowerId",
                table: "ProfileFollowerMappings",
                newName: "IX_ProfileFollowerMappings_FollowerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProfileFriendMappings",
                table: "ProfileFriendMappings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProfileFollowerMappings",
                table: "ProfileFollowerMappings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileFollowerMappings_UserProfiles_FollowerId",
                table: "ProfileFollowerMappings",
                column: "FollowerId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileFollowerMappings_UserProfiles_ProfileId",
                table: "ProfileFollowerMappings",
                column: "ProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileFriendMappings_UserProfiles_FriendId",
                table: "ProfileFriendMappings",
                column: "FriendId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileFriendMappings_UserProfiles_ProfileId",
                table: "ProfileFriendMappings",
                column: "ProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfileFollowerMappings_UserProfiles_FollowerId",
                table: "ProfileFollowerMappings");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileFollowerMappings_UserProfiles_ProfileId",
                table: "ProfileFollowerMappings");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileFriendMappings_UserProfiles_FriendId",
                table: "ProfileFriendMappings");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileFriendMappings_UserProfiles_ProfileId",
                table: "ProfileFriendMappings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProfileFriendMappings",
                table: "ProfileFriendMappings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProfileFollowerMappings",
                table: "ProfileFollowerMappings");

            migrationBuilder.RenameTable(
                name: "ProfileFriendMappings",
                newName: "ProfileFriendMapping");

            migrationBuilder.RenameTable(
                name: "ProfileFollowerMappings",
                newName: "ProfileFollowerMapping");

            migrationBuilder.RenameIndex(
                name: "IX_ProfileFriendMappings_ProfileId",
                table: "ProfileFriendMapping",
                newName: "IX_ProfileFriendMapping_ProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_ProfileFriendMappings_FriendId",
                table: "ProfileFriendMapping",
                newName: "IX_ProfileFriendMapping_FriendId");

            migrationBuilder.RenameIndex(
                name: "IX_ProfileFollowerMappings_ProfileId",
                table: "ProfileFollowerMapping",
                newName: "IX_ProfileFollowerMapping_ProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_ProfileFollowerMappings_FollowerId",
                table: "ProfileFollowerMapping",
                newName: "IX_ProfileFollowerMapping_FollowerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProfileFriendMapping",
                table: "ProfileFriendMapping",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProfileFollowerMapping",
                table: "ProfileFollowerMapping",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileFollowerMapping_UserProfiles_FollowerId",
                table: "ProfileFollowerMapping",
                column: "FollowerId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileFollowerMapping_UserProfiles_ProfileId",
                table: "ProfileFollowerMapping",
                column: "ProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileFriendMapping_UserProfiles_FriendId",
                table: "ProfileFriendMapping",
                column: "FriendId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileFriendMapping_UserProfiles_ProfileId",
                table: "ProfileFriendMapping",
                column: "ProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id");
        }
    }
}
