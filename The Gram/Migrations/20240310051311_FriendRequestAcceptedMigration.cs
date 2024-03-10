﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheGram.Migrations
{
    /// <inheritdoc />
    public partial class FriendRequestAcceptedMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isAccepted",
                table: "ProfileFriendMappings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isAccepted",
                table: "ProfileFriendMappings");
        }
    }
}
