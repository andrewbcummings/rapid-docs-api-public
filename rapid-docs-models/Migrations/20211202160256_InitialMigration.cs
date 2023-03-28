using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rapid_docs_models.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SigningDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileGuid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileExtension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileSize = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsTemplate = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SigningDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SigningForms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SigningForms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SigningFormPages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PageOrder = table.Column<int>(type: "int", nullable: false),
                    SigningFormId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SigningFormPages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SigningFormPages_SigningForms_SigningFormId",
                        column: x => x.SigningFormId,
                        principalTable: "SigningForms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Signings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SigningFormId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsTemplate = table.Column<bool>(type: "bit", nullable: false),
                    ApiVersion = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DateCreated = table.Column<int>(type: "int", nullable: false),
                    DateSent = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateLastOpened = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NumberOfTimesOpened = table.Column<int>(type: "int", nullable: false),
                    UserHasStarted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Signings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Signings_SigningForms_SigningFormId",
                        column: x => x.SigningFormId,
                        principalTable: "SigningForms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InputFields",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DefaultValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Required = table.Column<bool>(type: "bit", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    SigningFormPageId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InputFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InputFields_SigningFormPages_SigningFormPageId",
                        column: x => x.SigningFormPageId,
                        principalTable: "SigningFormPages",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InputOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InputFieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InputOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InputOptions_InputFields_InputFieldId",
                        column: x => x.InputFieldId,
                        principalTable: "InputFields",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_InputFields_SigningFormPageId",
                table: "InputFields",
                column: "SigningFormPageId");

            migrationBuilder.CreateIndex(
                name: "IX_InputOptions_InputFieldId",
                table: "InputOptions",
                column: "InputFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_SigningFormPages_SigningFormId",
                table: "SigningFormPages",
                column: "SigningFormId");

            migrationBuilder.CreateIndex(
                name: "IX_Signings_SigningFormId",
                table: "Signings",
                column: "SigningFormId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InputOptions");

            migrationBuilder.DropTable(
                name: "SigningDocuments");

            migrationBuilder.DropTable(
                name: "Signings");

            migrationBuilder.DropTable(
                name: "InputFields");

            migrationBuilder.DropTable(
                name: "SigningFormPages");

            migrationBuilder.DropTable(
                name: "SigningForms");
        }
    }
}
