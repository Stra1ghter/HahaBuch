using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HahaBuch.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAbbreviationAddDescriptionToCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Abbreviation",
                table: "Categories");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Categories",
                type: "TEXT",
                maxLength: 120,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Categories");

            migrationBuilder.AddColumn<string>(
                name: "Abbreviation",
                table: "Categories",
                type: "TEXT",
                maxLength: 4,
                nullable: true);
        }
    }
}
