using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kitchen.Migrations
{
    /// <inheritdoc />
    public partial class paymentMethodEdit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentMethods",
                table: "PaymentMethods");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContactInfos",
                table: "ContactInfos");

            migrationBuilder.RenameTable(
                name: "PaymentMethods",
                newName: "paymentMethods");

            migrationBuilder.RenameTable(
                name: "ContactInfos",
                newName: "contactInfos");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "paymentMethods",
                newName: "TypeEn");

            migrationBuilder.AddColumn<string>(
                name: "TypeAr",
                table: "paymentMethods",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_paymentMethods",
                table: "paymentMethods",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_contactInfos",
                table: "contactInfos",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_paymentMethods",
                table: "paymentMethods");

            migrationBuilder.DropPrimaryKey(
                name: "PK_contactInfos",
                table: "contactInfos");

            migrationBuilder.DropColumn(
                name: "TypeAr",
                table: "paymentMethods");

            migrationBuilder.RenameTable(
                name: "paymentMethods",
                newName: "PaymentMethods");

            migrationBuilder.RenameTable(
                name: "contactInfos",
                newName: "ContactInfos");

            migrationBuilder.RenameColumn(
                name: "TypeEn",
                table: "PaymentMethods",
                newName: "Type");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentMethods",
                table: "PaymentMethods",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContactInfos",
                table: "ContactInfos",
                column: "Id");
        }
    }
}
