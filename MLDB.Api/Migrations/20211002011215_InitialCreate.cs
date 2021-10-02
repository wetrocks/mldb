using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MLDB.Api.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LitterTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OsparID = table.Column<int>(nullable: false),
                    DadID = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LitterTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    IdpId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    EmailVerified = table.Column<bool>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.IdpId);
                });

            migrationBuilder.CreateTable(
                name: "Sites",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CreateUserIdpId = table.Column<string>(nullable: true),
                    CreateTimestamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sites_Users_CreateUserIdpId",
                        column: x => x.CreateUserIdpId,
                        principalTable: "Users",
                        principalColumn: "IdpId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Surveys",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Coordinator = table.Column<string>(nullable: true),
                    StartTimeStamp = table.Column<DateTime>(nullable: false),
                    EndTimeStamp = table.Column<DateTime>(nullable: false),
                    VolunteerCount = table.Column<short>(nullable: false),
                    TotalKg = table.Column<decimal>(nullable: false),
                    SiteId = table.Column<Guid>(nullable: false),
                    CreateUserIdpId = table.Column<string>(nullable: true),
                    CreateTimestamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Surveys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Surveys_Users_CreateUserIdpId",
                        column: x => x.CreateUserIdpId,
                        principalTable: "Users",
                        principalColumn: "IdpId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Surveys_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LitterItem",
                columns: table => new
                {
                    SurveyId = table.Column<Guid>(nullable: false),
                    LitterTypeId = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LitterItem", x => new { x.SurveyId, x.LitterTypeId });
                    table.ForeignKey(
                        name: "FK_LitterItem_LitterTypes_LitterTypeId",
                        column: x => x.LitterTypeId,
                        principalTable: "LitterTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LitterItem_Surveys_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "Surveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LitterItem_LitterTypeId",
                table: "LitterItem",
                column: "LitterTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sites_CreateUserIdpId",
                table: "Sites",
                column: "CreateUserIdpId");

            migrationBuilder.CreateIndex(
                name: "IX_Surveys_CreateUserIdpId",
                table: "Surveys",
                column: "CreateUserIdpId");

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
                name: "LitterTypes");

            migrationBuilder.DropTable(
                name: "Surveys");

            migrationBuilder.DropTable(
                name: "Sites");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
