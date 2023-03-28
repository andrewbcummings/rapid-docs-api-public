using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rapid_docs_models.Migrations
{
    public partial class IsSigningCompletedField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "Signings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "Signings");
        }
    }
}
