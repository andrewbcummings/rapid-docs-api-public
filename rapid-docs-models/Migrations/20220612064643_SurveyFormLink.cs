using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rapid_docs_models.Migrations
{
    public partial class SurveyFormLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateLastOpened",
                table: "Surveys",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateSent",
                table: "Surveys",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsTemplate",
                table: "Surveys",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfTimesOpened",
                table: "Surveys",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SurveyFormId",
                table: "Surveys",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "UserHasStarted",
                table: "Surveys",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Surveys_SurveyFormId",
                table: "Surveys",
                column: "SurveyFormId");

            migrationBuilder.AddForeignKey(
                name: "FK_Surveys_SurveyForms_SurveyFormId",
                table: "Surveys",
                column: "SurveyFormId",
                principalTable: "SurveyForms",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Surveys_SurveyForms_SurveyFormId",
                table: "Surveys");

            migrationBuilder.DropIndex(
                name: "IX_Surveys_SurveyFormId",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "DateLastOpened",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "DateSent",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "IsTemplate",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "NumberOfTimesOpened",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "SurveyFormId",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "UserHasStarted",
                table: "Surveys");
        }
    }
}
