using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MLDB.Api.Migrations
{
    public partial class LitterTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LitterTypeLitterTypesList");

            migrationBuilder.DropTable(
                name: "LitterTypeSurveyTemplate");

            migrationBuilder.DropTable(
                name: "LitterTypesList");

            migrationBuilder.DropTable(
                name: "LitterTypes");

            migrationBuilder.DropTable(
                name: "SurveyTemplates");

            migrationBuilder.CreateTable(
                name: "JointListLitterTypes",
                columns: table => new
                {
                    TypeCode = table.Column<string>(type: "TEXT", nullable: false),
                    J_Code = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Definition = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JointListLitterTypes", x => x.TypeCode);
                });

            migrationBuilder.CreateTable(
                name: "LitterSourceCategory",
                columns: table => new
                {
                    Id = table.Column<ushort>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LitterSourceCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OsparLitterTypes",
                columns: table => new
                {
                    Code = table.Column<uint>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Category = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OsparLitterTypes", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<ushort>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdpId = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    EmailVerified = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreateTimestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTimestamp = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InternalLitterTypes",
                columns: table => new
                {
                    Code = table.Column<uint>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    SourceCategoryId = table.Column<ushort>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternalLitterTypes", x => x.Code);
                    table.ForeignKey(
                        name: "FK_InternalLitterTypes_LitterSourceCategory_SourceCategoryId",
                        column: x => x.SourceCategoryId,
                        principalTable: "LitterSourceCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LitterTypeMapping_JointList",
                columns: table => new
                {
                    InternalLitterTypeCode = table.Column<uint>(type: "INTEGER", nullable: false),
                    MappedTypeKey = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LitterTypeMapping_JointList", x => x.InternalLitterTypeCode);
                    table.ForeignKey(
                        name: "FK_LitterTypeMapping_JointList_InternalLitterTypes_InternalLitterTypeCode",
                        column: x => x.InternalLitterTypeCode,
                        principalTable: "InternalLitterTypes",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LitterTypeMapping_JointList_JointListLitterTypes_MappedTypeKey",
                        column: x => x.MappedTypeKey,
                        principalTable: "JointListLitterTypes",
                        principalColumn: "TypeCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LitterTypeMapping_Ospar",
                columns: table => new
                {
                    InternalLitterTypeCode = table.Column<uint>(type: "INTEGER", nullable: false),
                    MappedTypeKey = table.Column<uint>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LitterTypeMapping_Ospar", x => x.InternalLitterTypeCode);
                    table.ForeignKey(
                        name: "FK_LitterTypeMapping_Ospar_InternalLitterTypes_InternalLitterTypeCode",
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

            migrationBuilder.InsertData(
                table: "JointListLitterTypes",
                columns: new[] { "TypeCode", "Definition", "Description", "J_Code" },
                values: new object[] { "pl_nn_bag_cabg_", "Shopping bags are medium-sized bags, typically around 10-20 litres in volume (though much larger versions exist, especially for non-grocery shopping), that are used by shoppers to carry home their purchases. Shopping bags can be made with a variety of plastics.", null, "J3" });

            migrationBuilder.InsertData(
                table: "JointListLitterTypes",
                columns: new[] { "TypeCode", "Definition", "Description", "J_Code" },
                values: new object[] { "another_one", "Another JLIST description", null, "J42" });

            migrationBuilder.InsertData(
                table: "LitterSourceCategory",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { (ushort)1, "Stand up paddleboards", "SUP" });

            migrationBuilder.InsertData(
                table: "LitterSourceCategory",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { (ushort)2, "Text about fisheries", "Fisheries" });

            migrationBuilder.InsertData(
                table: "OsparLitterTypes",
                columns: new[] { "Code", "Category", "Description" },
                values: new object[] { 42u, "Plastic", "Ospar Bags" });

            migrationBuilder.InsertData(
                table: "OsparLitterTypes",
                columns: new[] { "Code", "Category", "Description" },
                values: new object[] { 43u, "Metal", "Ospar Caps/Lids" });

            migrationBuilder.InsertData(
                table: "OsparLitterTypes",
                columns: new[] { "Code", "Category", "Description" },
                values: new object[] { 44u, "Glass", "Ospar Bottle" });

            migrationBuilder.InsertData(
                table: "OsparLitterTypes",
                columns: new[] { "Code", "Category", "Description" },
                values: new object[] { 45u, "Polystyrene", "Ospar Styrofoam" });

            migrationBuilder.InsertData(
                table: "InternalLitterTypes",
                columns: new[] { "Code", "Description", "SourceCategoryId" },
                values: new object[] { 1u, "Bags", (ushort)1 });

            migrationBuilder.InsertData(
                table: "InternalLitterTypes",
                columns: new[] { "Code", "Description", "SourceCategoryId" },
                values: new object[] { 2u, "Caps/Lids", (ushort)1 });

            migrationBuilder.InsertData(
                table: "InternalLitterTypes",
                columns: new[] { "Code", "Description", "SourceCategoryId" },
                values: new object[] { 3u, "Bottle", (ushort)1 });

            migrationBuilder.InsertData(
                table: "InternalLitterTypes",
                columns: new[] { "Code", "Description", "SourceCategoryId" },
                values: new object[] { 4u, "Styrofoam", (ushort)2 });

            migrationBuilder.InsertData(
                table: "LitterTypeMapping_JointList",
                columns: new[] { "InternalLitterTypeCode", "MappedTypeKey" },
                values: new object[] { 1u, "pl_nn_bag_cabg_" });

            migrationBuilder.InsertData(
                table: "LitterTypeMapping_Ospar",
                columns: new[] { "InternalLitterTypeCode", "MappedTypeKey" },
                values: new object[] { 1u, 42u });

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
                name: "IX_Users_IdpId",
                table: "Users",
                column: "IdpId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LitterTypeMapping_JointList");

            migrationBuilder.DropTable(
                name: "LitterTypeMapping_Ospar");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "JointListLitterTypes");

            migrationBuilder.DropTable(
                name: "InternalLitterTypes");

            migrationBuilder.DropTable(
                name: "OsparLitterTypes");

            migrationBuilder.DropTable(
                name: "LitterSourceCategory");

            migrationBuilder.CreateTable(
                name: "LitterTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DadId = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    OsparId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LitterTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LitterTypesList",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LitterTypesList", x => x.Id);
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
                name: "LitterTypeLitterTypesList",
                columns: table => new
                {
                    LitterTypesId = table.Column<int>(type: "INTEGER", nullable: false),
                    LitterTypesListId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LitterTypeLitterTypesList", x => new { x.LitterTypesId, x.LitterTypesListId });
                    table.ForeignKey(
                        name: "FK_LitterTypeLitterTypesList_LitterTypes_LitterTypesId",
                        column: x => x.LitterTypesId,
                        principalTable: "LitterTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LitterTypeLitterTypesList_LitterTypesList_LitterTypesListId",
                        column: x => x.LitterTypesListId,
                        principalTable: "LitterTypesList",
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
                        name: "FK_LitterTypeSurveyTemplate_LitterTypes_LitterTypesId",
                        column: x => x.LitterTypesId,
                        principalTable: "LitterTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LitterTypeSurveyTemplate_SurveyTemplates_SurveysId",
                        column: x => x.SurveysId,
                        principalTable: "SurveyTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "LitterTypes",
                columns: new[] { "Id", "DadId", "Description", "OsparId" },
                values: new object[] { 42, "1", "Bags", 1 });

            migrationBuilder.InsertData(
                table: "LitterTypes",
                columns: new[] { "Id", "DadId", "Description", "OsparId" },
                values: new object[] { 43, "2", "Caps/Lids", 2 });

            migrationBuilder.InsertData(
                table: "LitterTypes",
                columns: new[] { "Id", "DadId", "Description", "OsparId" },
                values: new object[] { 44, "3", "Bottle", 3 });

            migrationBuilder.InsertData(
                table: "LitterTypes",
                columns: new[] { "Id", "DadId", "Description", "OsparId" },
                values: new object[] { 45, "4", "Styrofoam", 4 });

            migrationBuilder.InsertData(
                table: "LitterTypesList",
                columns: new[] { "Id", "Description" },
                values: new object[] { "1.0", "Initial litter types" });

            migrationBuilder.InsertData(
                table: "LitterTypeLitterTypesList",
                columns: new[] { "LitterTypesId", "LitterTypesListId" },
                values: new object[] { 42, "1.0" });

            migrationBuilder.InsertData(
                table: "LitterTypeLitterTypesList",
                columns: new[] { "LitterTypesId", "LitterTypesListId" },
                values: new object[] { 43, "1.0" });

            migrationBuilder.InsertData(
                table: "LitterTypeLitterTypesList",
                columns: new[] { "LitterTypesId", "LitterTypesListId" },
                values: new object[] { 44, "1.0" });

            migrationBuilder.InsertData(
                table: "LitterTypeLitterTypesList",
                columns: new[] { "LitterTypesId", "LitterTypesListId" },
                values: new object[] { 45, "1.0" });

            migrationBuilder.CreateIndex(
                name: "IX_LitterTypeLitterTypesList_LitterTypesListId",
                table: "LitterTypeLitterTypesList",
                column: "LitterTypesListId");

            migrationBuilder.CreateIndex(
                name: "IX_LitterTypeSurveyTemplate_SurveysId",
                table: "LitterTypeSurveyTemplate",
                column: "SurveysId");
        }
    }
}
