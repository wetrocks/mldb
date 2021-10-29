using Microsoft.EntityFrameworkCore.Migrations;

namespace MLDB.Api.Migrations
{
    public partial class seedJson3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "LitterTypes",
                keyColumn: "Id",
                keyValue: 42,
                column: "DadID",
                value: "1");

            migrationBuilder.UpdateData(
                table: "LitterTypes",
                keyColumn: "Id",
                keyValue: 43,
                column: "DadID",
                value: "2");

            migrationBuilder.UpdateData(
                table: "LitterTypes",
                keyColumn: "Id",
                keyValue: 44,
                column: "DadID",
                value: "3");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "LitterTypes",
                keyColumn: "Id",
                keyValue: 42,
                column: "DadID",
                value: null);

            migrationBuilder.UpdateData(
                table: "LitterTypes",
                keyColumn: "Id",
                keyValue: 43,
                column: "DadID",
                value: null);

            migrationBuilder.UpdateData(
                table: "LitterTypes",
                keyColumn: "Id",
                keyValue: 44,
                column: "DadID",
                value: null);
        }
    }
}
