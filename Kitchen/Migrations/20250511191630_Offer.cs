using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kitchen.Migrations
{
    /// <inheritdoc />
    public partial class Offer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_sectionContents",
                table: "sectionContents");

            migrationBuilder.RenameTable(
                name: "sectionContents",
                newName: "SectionContents");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SectionContents",
                table: "SectionContents",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SectionContents",
                table: "SectionContents");

            migrationBuilder.RenameTable(
                name: "SectionContents",
                newName: "sectionContents");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sectionContents",
                table: "sectionContents",
                column: "Id");
        }
    }
}
