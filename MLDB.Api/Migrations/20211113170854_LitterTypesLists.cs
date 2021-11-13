using Microsoft.EntityFrameworkCore.Migrations;

namespace MLDB.Api.Migrations
{
    public partial class LitterTypesLists : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LitterTypeSurveyTemplate_LitterType_LitterTypesId",
                table: "LitterTypeSurveyTemplate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LitterType",
                table: "LitterType");

            migrationBuilder.RenameTable(
                name: "LitterType",
                newName: "LitterTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LitterTypes",
                table: "LitterTypes",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_LitterTypeSurveyTemplate_LitterTypes_LitterTypesId",
                table: "LitterTypeSurveyTemplate",
                column: "LitterTypesId",
                principalTable: "LitterTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LitterTypeSurveyTemplate_LitterTypes_LitterTypesId",
                table: "LitterTypeSurveyTemplate");

            migrationBuilder.DropTable(
                name: "LitterTypeLitterTypesList");

            migrationBuilder.DropTable(
                name: "LitterTypesList");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LitterTypes",
                table: "LitterTypes");

            migrationBuilder.DeleteData(
                table: "LitterTypes",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "LitterTypes",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "LitterTypes",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "LitterTypes",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.RenameTable(
                name: "LitterTypes",
                newName: "LitterType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LitterType",
                table: "LitterType",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LitterTypeSurveyTemplate_LitterType_LitterTypesId",
                table: "LitterTypeSurveyTemplate",
                column: "LitterTypesId",
                principalTable: "LitterType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
