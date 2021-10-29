using Microsoft.EntityFrameworkCore.Migrations;

namespace MLDB.Api.Migrations
{
    public partial class seedJson2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "LitterTypes",
                columns: new[] { "Id", "DadID", "Description", "OsparID" },
                values: new object[] { 44, null, "Bottle", 3 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LitterTypes",
                keyColumn: "Id",
                keyValue: 44);
        }
    }
}
