using Microsoft.EntityFrameworkCore.Migrations;

namespace SolarRadiationStore.Lib.Migrations
{
    public partial class mapforecaststotableForecasts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DbSolradForecast_Locations_LocationForecastsId",
                table: "DbSolradForecast");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DbSolradForecast",
                table: "DbSolradForecast");

            migrationBuilder.RenameTable(
                name: "DbSolradForecast",
                newName: "Forecasts");

            migrationBuilder.RenameIndex(
                name: "IX_DbSolradForecast_LocationForecastsId",
                table: "Forecasts",
                newName: "IX_Forecasts_LocationForecastsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Forecasts",
                table: "Forecasts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Forecasts_Locations_LocationForecastsId",
                table: "Forecasts",
                column: "LocationForecastsId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Forecasts_Locations_LocationForecastsId",
                table: "Forecasts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Forecasts",
                table: "Forecasts");

            migrationBuilder.RenameTable(
                name: "Forecasts",
                newName: "DbSolradForecast");

            migrationBuilder.RenameIndex(
                name: "IX_Forecasts_LocationForecastsId",
                table: "DbSolradForecast",
                newName: "IX_DbSolradForecast_LocationForecastsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DbSolradForecast",
                table: "DbSolradForecast",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DbSolradForecast_Locations_LocationForecastsId",
                table: "DbSolradForecast",
                column: "LocationForecastsId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
