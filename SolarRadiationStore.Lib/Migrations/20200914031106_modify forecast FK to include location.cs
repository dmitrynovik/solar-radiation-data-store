using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SolarRadiationStore.Lib.Migrations
{
    public partial class modifyforecastFKtoincludelocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SolradForecast");

            migrationBuilder.CreateTable(
                name: "DbSolradForecast",
                columns: table => new
                {
                    PeriodEnd = table.Column<DateTime>(nullable: false),
                    Period = table.Column<TimeSpan>(nullable: false),
                    LocationForecastId = table.Column<int>(nullable: false),
                    Ghi = table.Column<int>(nullable: false),
                    Ghi90 = table.Column<int>(nullable: false),
                    Ghi10 = table.Column<int>(nullable: false),
                    ClearSkyGhi = table.Column<int>(nullable: false),
                    ClearSkyDni = table.Column<int>(nullable: false),
                    ClearSkyDhi = table.Column<int>(nullable: false),
                    Ebh = table.Column<int>(nullable: false),
                    Ebh10 = table.Column<int>(nullable: false),
                    Ebh90 = table.Column<int>(nullable: false),
                    Dni = table.Column<int>(nullable: false),
                    Dni10 = table.Column<int>(nullable: false),
                    Dni90 = table.Column<int>(nullable: false),
                    Dhi = table.Column<int>(nullable: false),
                    Dhi10 = table.Column<int>(nullable: false),
                    Dhi90 = table.Column<int>(nullable: false),
                    AirTemp = table.Column<int>(nullable: false),
                    Zenith = table.Column<int>(nullable: false),
                    Azimuth = table.Column<int>(nullable: false),
                    CloudOpacity = table.Column<int>(nullable: false),
                    SnowClearnessRooftop = table.Column<int>(nullable: true),
                    SnowClearnessUtility = table.Column<int>(nullable: true),
                    LocationForecastsId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbSolradForecast", x => new { x.LocationForecastId, x.PeriodEnd, x.Period });
                    table.ForeignKey(
                        name: "FK_DbSolradForecast_Locations_LocationForecastsId",
                        column: x => x.LocationForecastsId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DbSolradForecast_LocationForecastsId",
                table: "DbSolradForecast",
                column: "LocationForecastsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DbSolradForecast");

            migrationBuilder.CreateTable(
                name: "SolradForecast",
                columns: table => new
                {
                    PeriodEnd = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Period = table.Column<TimeSpan>(type: "interval", nullable: false),
                    AirTemp = table.Column<int>(type: "integer", nullable: false),
                    Azimuth = table.Column<int>(type: "integer", nullable: false),
                    ClearSkyDhi = table.Column<int>(type: "integer", nullable: false),
                    ClearSkyDni = table.Column<int>(type: "integer", nullable: false),
                    ClearSkyGhi = table.Column<int>(type: "integer", nullable: false),
                    CloudOpacity = table.Column<int>(type: "integer", nullable: false),
                    Dhi = table.Column<int>(type: "integer", nullable: false),
                    Dhi10 = table.Column<int>(type: "integer", nullable: false),
                    Dhi90 = table.Column<int>(type: "integer", nullable: false),
                    Dni = table.Column<int>(type: "integer", nullable: false),
                    Dni10 = table.Column<int>(type: "integer", nullable: false),
                    Dni90 = table.Column<int>(type: "integer", nullable: false),
                    Ebh = table.Column<int>(type: "integer", nullable: false),
                    Ebh10 = table.Column<int>(type: "integer", nullable: false),
                    Ebh90 = table.Column<int>(type: "integer", nullable: false),
                    Ghi = table.Column<int>(type: "integer", nullable: false),
                    Ghi10 = table.Column<int>(type: "integer", nullable: false),
                    Ghi90 = table.Column<int>(type: "integer", nullable: false),
                    LocationForecastsId = table.Column<long>(type: "bigint", nullable: true),
                    SnowClearnessRooftop = table.Column<int>(type: "integer", nullable: true),
                    SnowClearnessUtility = table.Column<int>(type: "integer", nullable: true),
                    Zenith = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolradForecast", x => new { x.PeriodEnd, x.Period });
                    table.ForeignKey(
                        name: "FK_SolradForecast_Locations_LocationForecastsId",
                        column: x => x.LocationForecastsId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SolradForecast_LocationForecastsId",
                table: "SolradForecast",
                column: "LocationForecastsId");
        }
    }
}
