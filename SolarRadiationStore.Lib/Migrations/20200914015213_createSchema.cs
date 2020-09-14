using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SolarRadiationStore.Lib.Migrations
{
    public partial class createSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.CreateTable(
                name: "Forecasts",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Location = table.Column<Point>(type: "geography (point)", nullable: true),
                    Srid = table.Column<long>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forecasts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SolradForecast",
                columns: table => new
                {
                    PeriodEnd = table.Column<DateTime>(nullable: false),
                    Period = table.Column<TimeSpan>(nullable: false),
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
                    table.PrimaryKey("PK_SolradForecast", x => new { x.PeriodEnd, x.Period });
                    table.ForeignKey(
                        name: "FK_SolradForecast_Forecasts_LocationForecastsId",
                        column: x => x.LocationForecastsId,
                        principalTable: "Forecasts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Forecasts_Location",
                table: "Forecasts",
                column: "Location")
                .Annotation("Npgsql:IndexMethod", "GIST");

            migrationBuilder.CreateIndex(
                name: "IX_SolradForecast_LocationForecastsId",
                table: "SolradForecast",
                column: "LocationForecastsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SolradForecast");

            migrationBuilder.DropTable(
                name: "Forecasts");
        }
    }
}
