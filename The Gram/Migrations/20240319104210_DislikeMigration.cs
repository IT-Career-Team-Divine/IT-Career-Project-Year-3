using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheGram.Migrations
{
    /// <inheritdoc />
    public partial class DislikeMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Reactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TotalDislikes",
                table: "Content",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Reactions");

            migrationBuilder.DropColumn(
                name: "TotalDislikes",
                table: "Content");
        }
    }
}
