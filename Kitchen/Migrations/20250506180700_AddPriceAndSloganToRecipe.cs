using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kitchen.Migrations
{
    /// <inheritdoc />
    public partial class AddPriceAndSloganToRecipe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Recipes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Slogan",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Slogan",
                table: "Recipes");
        }
    }
}
