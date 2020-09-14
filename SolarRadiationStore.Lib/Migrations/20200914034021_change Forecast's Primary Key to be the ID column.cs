using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SolarRadiationStore.Lib.Migrations
{
    public partial class changeForecastsPrimaryKeytobetheIDcolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DbSolradForecast",
                table: "DbSolradForecast");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "DbSolradForecast",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DbSolradForecast",
                table: "DbSolradForecast",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DbSolradForecast",
                table: "DbSolradForecast");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "DbSolradForecast");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DbSolradForecast",
                table: "DbSolradForecast",
                columns: new[] { "LocationForecastId", "PeriodEnd", "Period" });
        }
    }
}
