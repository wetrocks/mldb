using Microsoft.EntityFrameworkCore.Migrations;

namespace MLDB.Api.Migrations
{
    public partial class seedJson5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "LitterTypes",
                columns: new[] { "Id", "DadID", "Description", "OsparID" },
                values: new object[] { 45, "4", "Styrofoam", 4 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LitterTypes",
                keyColumn: "Id",
                keyValue: 45);
        }
    }
}
