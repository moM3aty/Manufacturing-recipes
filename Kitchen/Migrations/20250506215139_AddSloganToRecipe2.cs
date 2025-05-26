using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kitchen.Migrations
{
    /// <inheritdoc />
    public partial class AddSloganToRecipe2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Slogan",
                table: "Recipes",
                newName: "SloganEn");

            migrationBuilder.AddColumn<string>(
                name: "SloganAr",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SloganAr",
                table: "Recipes");

            migrationBuilder.RenameColumn(
                name: "SloganEn",
                table: "Recipes",
                newName: "Slogan");
        }
    }
}
