using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rapid_docs_models.Migrations
{
    public partial class SigingDocFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerAccountId",
                table: "Signings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TemplateName",
                table: "Signings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FileUrl",
                table: "SigningDocuments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SigningId",
                table: "SigningDocuments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Signings_CustomerAccountId",
                table: "Signings",
                column: "CustomerAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_SigningDocuments_SigningId",
                table: "SigningDocuments",
                column: "SigningId");

            migrationBuilder.AddForeignKey(
                name: "FK_SigningDocuments_Signings_SigningId",
                table: "SigningDocuments",
                column: "SigningId",
                principalTable: "Signings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Signings_CustomerAccounts_CustomerAccountId",
                table: "Signings",
                column: "CustomerAccountId",
                principalTable: "CustomerAccounts",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SigningDocuments_Signings_SigningId",
                table: "SigningDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_Signings_CustomerAccounts_CustomerAccountId",
                table: "Signings");

            migrationBuilder.DropIndex(
                name: "IX_Signings_CustomerAccountId",
                table: "Signings");

            migrationBuilder.DropIndex(
                name: "IX_SigningDocuments_SigningId",
                table: "SigningDocuments");

            migrationBuilder.DropColumn(
                name: "CustomerAccountId",
                table: "Signings");

            migrationBuilder.DropColumn(
                name: "TemplateName",
                table: "Signings");

            migrationBuilder.DropColumn(
                name: "FileUrl",
                table: "SigningDocuments");

            migrationBuilder.DropColumn(
                name: "SigningId",
                table: "SigningDocuments");
        }
    }
}
