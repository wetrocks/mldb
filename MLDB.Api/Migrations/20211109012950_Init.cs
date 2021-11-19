using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MLDB.Api.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LitterType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OsparId = table.Column<int>(type: "INTEGER", nullable: false),
                    DadId = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LitterType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sites",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    CreateUserId = table.Column<string>(type: "TEXT", nullable: true),
                    CreateTimestamp = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sites", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SurveyTemplates",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Surveys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    SiteId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreateUserId = table.Column<string>(type: "TEXT", nullable: true),
                    CreateTimestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CoordinatorName = table.Column<string>(type: "TEXT", nullable: true),
                    StartTimeStamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndTimeStamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    VolunteerCount = table.Column<short>(type: "INTEGER", nullable: false),
                    TotalKg = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Surveys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Surveys_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LitterTypeSurveyTemplate",
                columns: table => new
                {
                    LitterTypesId = table.Column<int>(type: "INTEGER", nullable: false),
                    SurveysId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LitterTypeSurveyTemplate", x => new { x.LitterTypesId, x.SurveysId });
                    table.ForeignKey(
                        name: "FK_LitterTypeSurveyTemplate_LitterType_LitterTypesId",
                        column: x => x.LitterTypesId,
                        principalTable: "LitterType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LitterTypeSurveyTemplate_SurveyTemplates_SurveysId",
                        column: x => x.SurveysId,
                        principalTable: "SurveyTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LitterItem",
                columns: table => new
                {
                    LitterTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    SurveyId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Count = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LitterItem", x => new { x.SurveyId, x.LitterTypeId });
                    table.ForeignKey(
                        name: "FK_LitterItem_Surveys_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "Surveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LitterTypeSurveyTemplate_SurveysId",
                table: "LitterTypeSurveyTemplate",
                column: "SurveysId");

            migrationBuilder.CreateIndex(
                name: "IX_Surveys_SiteId",
                table: "Surveys",
                column: "SiteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LitterItem");

            migrationBuilder.DropTable(
                name: "LitterTypeSurveyTemplate");

            migrationBuilder.DropTable(
                name: "Surveys");

            migrationBuilder.DropTable(
                name: "LitterType");

            migrationBuilder.DropTable(
                name: "SurveyTemplates");

            migrationBuilder.DropTable(
                name: "Sites");
        }
    }
}
