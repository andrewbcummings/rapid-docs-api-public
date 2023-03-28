using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rapid_docs_models.Migrations
{
    public partial class CompanyLogoGuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CompanyLogo",
                table: "CustomerAccounts",
                newName: "CompanyLogoUrl");

            migrationBuilder.AddColumn<string>(
                name: "CompanyLogoExtension",
                table: "CustomerAccounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompanyLogoGuid",
                table: "CustomerAccounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyLogoExtension",
                table: "CustomerAccounts");

            migrationBuilder.DropColumn(
                name: "CompanyLogoGuid",
                table: "CustomerAccounts");

            migrationBuilder.RenameColumn(
                name: "CompanyLogoUrl",
                table: "CustomerAccounts",
                newName: "CompanyLogo");
        }
    }
}
