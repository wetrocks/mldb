using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MLDB.Api.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JointListLitterTypes",
                columns: table => new
                {
                    TypeCode = table.Column<string>(type: "text", nullable: false),
                    J_Code = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Definition = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JointListLitterTypes", x => x.TypeCode);
                });

            migrationBuilder.CreateTable(
                name: "LitterSourceCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LitterSourceCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OsparLitterTypes",
                columns: table => new
                {
                    Code = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Category = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OsparLitterTypes", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Sites",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    CreateUserId = table.Column<string>(type: "text", nullable: true),
                    CreateTimestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sites", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdpId = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    EmailVerified = table.Column<bool>(type: "boolean", nullable: false),
                    CreateTimestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateTimestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InternalLitterTypes",
                columns: table => new
                {
                    Code = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: true),
                    SourceCategoryId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternalLitterTypes", x => x.Code);
                    table.ForeignKey(
                        name: "FK_InternalLitterTypes_LitterSourceCategory_SourceCategoryId",
                        column: x => x.SourceCategoryId,
                        principalTable: "LitterSourceCategory",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Surveys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SiteId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateUserId = table.Column<string>(type: "text", nullable: true),
                    CreateTimestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CoordinatorName = table.Column<string>(type: "text", nullable: true),
                    SurveyDate = table.Column<DateOnly>(type: "date", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    VolunteerCount = table.Column<short>(type: "smallint", nullable: false),
                    TotalKg = table.Column<decimal>(type: "numeric", nullable: false)
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
                name: "LitterTypeMapping_JointList",
                columns: table => new
                {
                    InternalLitterTypeCode = table.Column<long>(type: "bigint", nullable: false),
                    MappedTypeKey = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LitterTypeMapping_JointList", x => x.InternalLitterTypeCode);
                    table.ForeignKey(
                        name: "FK_LitterTypeMapping_JointList_InternalLitterTypes_InternalLit~",
                        column: x => x.InternalLitterTypeCode,
                        principalTable: "InternalLitterTypes",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LitterTypeMapping_JointList_JointListLitterTypes_MappedType~",
                        column: x => x.MappedTypeKey,
                        principalTable: "JointListLitterTypes",
                        principalColumn: "TypeCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LitterTypeMapping_Ospar",
                columns: table => new
                {
                    InternalLitterTypeCode = table.Column<long>(type: "bigint", nullable: false),
                    MappedTypeKey = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LitterTypeMapping_Ospar", x => x.InternalLitterTypeCode);
                    table.ForeignKey(
                        name: "FK_LitterTypeMapping_Ospar_InternalLitterTypes_InternalLitterT~",
                        column: x => x.InternalLitterTypeCode,
                        principalTable: "InternalLitterTypes",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LitterTypeMapping_Ospar_OsparLitterTypes_MappedTypeKey",
                        column: x => x.MappedTypeKey,
                        principalTable: "OsparLitterTypes",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LitterItem",
                columns: table => new
                {
                    LitterTypeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SurveyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Count = table.Column<int>(type: "integer", nullable: false)
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

            migrationBuilder.InsertData(
                table: "JointListLitterTypes",
                columns: new[] { "TypeCode", "Definition", "Description", "J_Code" },
                values: new object[,]
                {
                    { "another_one", "Another JLIST description", null, "J42" },
                    { "pl_nn_bag_cabg_", "Shopping bags are medium-sized bags, typically around 10-20 litres in volume (though much larger versions exist, especially for non-grocery shopping), that are used by shoppers to carry home their purchases. Shopping bags can be made with a variety of plastics.", null, "J3" }
                });

            migrationBuilder.InsertData(
                table: "LitterSourceCategory",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Stand up paddleboards", "SUP" },
                    { 2, "Text about fisheries", "Fisheries" }
                });

            migrationBuilder.InsertData(
                table: "OsparLitterTypes",
                columns: new[] { "Code", "Category", "Description" },
                values: new object[,]
                {
                    { 42L, "Plastic", "Ospar Bags" },
                    { 43L, "Metal", "Ospar Caps/Lids" },
                    { 44L, "Glass", "Ospar Bottle" },
                    { 45L, "Polystyrene", "Ospar Styrofoam" }
                });

            migrationBuilder.InsertData(
                table: "InternalLitterTypes",
                columns: new[] { "Code", "Description", "SourceCategoryId" },
                values: new object[,]
                {
                    { 1L, "Bags", 1 },
                    { 2L, "Caps/Lids", 1 },
                    { 3L, "Bottle", 1 },
                    { 4L, "Styrofoam", 2 }
                });

            migrationBuilder.InsertData(
                table: "LitterTypeMapping_JointList",
                columns: new[] { "InternalLitterTypeCode", "MappedTypeKey" },
                values: new object[] { 1L, "pl_nn_bag_cabg_" });

            migrationBuilder.InsertData(
                table: "LitterTypeMapping_Ospar",
                columns: new[] { "InternalLitterTypeCode", "MappedTypeKey" },
                values: new object[] { 1L, 42L });

            migrationBuilder.CreateIndex(
                name: "IX_InternalLitterTypes_SourceCategoryId",
                table: "InternalLitterTypes",
                column: "SourceCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_LitterTypeMapping_JointList_MappedTypeKey",
                table: "LitterTypeMapping_JointList",
                column: "MappedTypeKey");

            migrationBuilder.CreateIndex(
                name: "IX_LitterTypeMapping_Ospar_MappedTypeKey",
                table: "LitterTypeMapping_Ospar",
                column: "MappedTypeKey");

            migrationBuilder.CreateIndex(
                name: "IX_Surveys_SiteId",
                table: "Surveys",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IdpId",
                table: "Users",
                column: "IdpId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LitterItem");

            migrationBuilder.DropTable(
                name: "LitterTypeMapping_JointList");

            migrationBuilder.DropTable(
                name: "LitterTypeMapping_Ospar");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Surveys");

            migrationBuilder.DropTable(
                name: "JointListLitterTypes");

            migrationBuilder.DropTable(
                name: "InternalLitterTypes");

            migrationBuilder.DropTable(
                name: "OsparLitterTypes");

            migrationBuilder.DropTable(
                name: "Sites");

            migrationBuilder.DropTable(
                name: "LitterSourceCategory");
        }
    }
}
