using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheGram.Migrations
{
    /// <inheritdoc />
    public partial class IdMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfileFollowerMappings_UserProfiles_ProfileId",
                table: "ProfileFollowerMappings");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileFriendMappings_UserProfiles_ProfileId",
                table: "ProfileFriendMappings");

            migrationBuilder.AlterColumn<string>(
                name: "ProfileId",
                table: "ProfileFriendMappings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProfileId",
                table: "ProfileFollowerMappings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileFollowerMappings_UserProfiles_ProfileId",
                table: "ProfileFollowerMappings",
                column: "ProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileFriendMappings_UserProfiles_ProfileId",
                table: "ProfileFriendMappings",
                column: "ProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfileFollowerMappings_UserProfiles_ProfileId",
                table: "ProfileFollowerMappings");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileFriendMappings_UserProfiles_ProfileId",
                table: "ProfileFriendMappings");

            migrationBuilder.AlterColumn<string>(
                name: "ProfileId",
                table: "ProfileFriendMappings",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ProfileId",
                table: "ProfileFollowerMappings",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileFollowerMappings_UserProfiles_ProfileId",
                table: "ProfileFollowerMappings",
                column: "ProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileFriendMappings_UserProfiles_ProfileId",
                table: "ProfileFriendMappings",
                column: "ProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id");
        }
    }
}
