using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rapid_docs_models.Migrations
{
    public partial class SignerIpAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SignerGuid",
                table: "SigningDocuments");

            migrationBuilder.DropColumn(
                name: "SignerIpAddress",
                table: "SigningDocuments");

            migrationBuilder.AddColumn<string>(
                name: "SignerGuid",
                table: "SigningRecipientMappings",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SignerIpAddress",
                table: "SigningRecipientMappings",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SignerGuid",
                table: "SigningRecipientMappings");

            migrationBuilder.DropColumn(
                name: "SignerIpAddress",
                table: "SigningRecipientMappings");

            migrationBuilder.AddColumn<string>(
                name: "SignerGuid",
                table: "SigningDocuments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SignerIpAddress",
                table: "SigningDocuments",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }
    }
}
