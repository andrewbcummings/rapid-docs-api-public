using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rapid_docs_models.Migrations
{
    public partial class CompanyTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Signings_CustomerAccounts_CustomerAccountId",
                table: "Signings");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_CustomerAccounts_CustomerAccountId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "CustomerAccounts");

            migrationBuilder.RenameColumn(
                name: "CustomerAccountId",
                table: "Users",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_CustomerAccountId",
                table: "Users",
                newName: "IX_Users_CompanyId");

            migrationBuilder.RenameColumn(
                name: "CustomerAccountId",
                table: "Signings",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Signings_CustomerAccountId",
                table: "Signings",
                newName: "IX_Signings_CompanyId");

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyLogoUrl = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CompanyLogoGuid = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CompanyLogoExtension = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Signings_Companies_CompanyId",
                table: "Signings",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Companies_CompanyId",
                table: "Users",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Signings_Companies_CompanyId",
                table: "Signings");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Companies_CompanyId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "Users",
                newName: "CustomerAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_CompanyId",
                table: "Users",
                newName: "IX_Users_CustomerAccountId");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "Signings",
                newName: "CustomerAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Signings_CompanyId",
                table: "Signings",
                newName: "IX_Signings_CustomerAccountId");

            migrationBuilder.CreateTable(
                name: "CustomerAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyLogoExtension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyLogoGuid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyLogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerAccounts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAccounts_UserId",
                table: "CustomerAccounts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Signings_CustomerAccounts_CustomerAccountId",
                table: "Signings",
                column: "CustomerAccountId",
                principalTable: "CustomerAccounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_CustomerAccounts_CustomerAccountId",
                table: "Users",
                column: "CustomerAccountId",
                principalTable: "CustomerAccounts",
                principalColumn: "Id");
        }
    }
}
