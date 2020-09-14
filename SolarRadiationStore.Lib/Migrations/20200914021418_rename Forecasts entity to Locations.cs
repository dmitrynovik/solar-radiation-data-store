using Microsoft.EntityFrameworkCore.Migrations;

namespace SolarRadiationStore.Lib.Migrations
{
    public partial class renameForecastsentitytoLocations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SolradForecast_Forecasts_LocationForecastsId",
                table: "SolradForecast");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Forecasts",
                table: "Forecasts");

            migrationBuilder.RenameTable(
                name: "Forecasts",
                newName: "Locations");

            migrationBuilder.RenameIndex(
                name: "IX_Forecasts_Location",
                table: "Locations",
                newName: "IX_Locations_Location");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Locations",
                table: "Locations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SolradForecast_Locations_LocationForecastsId",
                table: "SolradForecast",
                column: "LocationForecastsId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SolradForecast_Locations_LocationForecastsId",
                table: "SolradForecast");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Locations",
                table: "Locations");

            migrationBuilder.RenameTable(
                name: "Locations",
                newName: "Forecasts");

            migrationBuilder.RenameIndex(
                name: "IX_Locations_Location",
                table: "Forecasts",
                newName: "IX_Forecasts_Location");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Forecasts",
                table: "Forecasts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SolradForecast_Forecasts_LocationForecastsId",
                table: "SolradForecast",
                column: "LocationForecastsId",
                principalTable: "Forecasts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
