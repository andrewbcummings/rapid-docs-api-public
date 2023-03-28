using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rapid_docs_models.Migrations
{
    public partial class CompanyLogo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyLogo",
                table: "CustomerAccounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "CustomerAccounts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAccounts_UserId",
                table: "CustomerAccounts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerAccounts_Users_UserId",
                table: "CustomerAccounts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerAccounts_Users_UserId",
                table: "CustomerAccounts");

            migrationBuilder.DropIndex(
                name: "IX_CustomerAccounts_UserId",
                table: "CustomerAccounts");

            migrationBuilder.DropColumn(
                name: "CompanyLogo",
                table: "CustomerAccounts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CustomerAccounts");
        }
    }
}
