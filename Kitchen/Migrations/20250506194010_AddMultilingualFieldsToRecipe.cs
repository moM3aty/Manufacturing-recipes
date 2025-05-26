using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kitchen.Migrations
{
    /// <inheritdoc />
    public partial class AddMultilingualFieldsToRecipe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Recipes",
                newName: "TitleEn");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Recipes",
                newName: "TitleAr");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEn",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "DescriptionEn",
                table: "Recipes");

            migrationBuilder.RenameColumn(
                name: "TitleEn",
                table: "Recipes",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "TitleAr",
                table: "Recipes",
                newName: "Description");
        }
    }
}
