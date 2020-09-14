﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SolarRadiationStore.Lib;

namespace SolarRadiationStore.Lib.Migrations
{
    [DbContext(typeof(SolarRadiationDataContext))]
    [Migration("20200914233019_convert Id to long")]
    partial class convertIdtolong
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:PostgresExtension:postgis", ",,")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("SolarRadiationStore.Lib.DbSolradForecast", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("AirTemp")
                        .HasColumnType("integer");

                    b.Property<int>("Azimuth")
                        .HasColumnType("integer");

                    b.Property<int>("ClearSkyDhi")
                        .HasColumnType("integer");

                    b.Property<int>("ClearSkyDni")
                        .HasColumnType("integer");

                    b.Property<int>("ClearSkyGhi")
                        .HasColumnType("integer");

                    b.Property<int>("CloudOpacity")
                        .HasColumnType("integer");

                    b.Property<int>("Dhi")
                        .HasColumnType("integer");

                    b.Property<int>("Dhi10")
                        .HasColumnType("integer");

                    b.Property<int>("Dhi90")
                        .HasColumnType("integer");

                    b.Property<int>("Dni")
                        .HasColumnType("integer");

                    b.Property<int>("Dni10")
                        .HasColumnType("integer");

                    b.Property<int>("Dni90")
                        .HasColumnType("integer");

                    b.Property<int>("Ebh")
                        .HasColumnType("integer");

                    b.Property<int>("Ebh10")
                        .HasColumnType("integer");

                    b.Property<int>("Ebh90")
                        .HasColumnType("integer");

                    b.Property<int>("Ghi")
                        .HasColumnType("integer");

                    b.Property<int>("Ghi10")
                        .HasColumnType("integer");

                    b.Property<int>("Ghi90")
                        .HasColumnType("integer");

                    b.Property<long>("LocationForecastId")
                        .HasColumnType("bigint");

                    b.Property<long?>("LocationForecastsId")
                        .HasColumnType("bigint");

                    b.Property<TimeSpan>("Period")
                        .HasColumnType("interval");

                    b.Property<DateTime>("PeriodEnd")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("SnowClearnessRooftop")
                        .HasColumnType("integer");

                    b.Property<int?>("SnowClearnessUtility")
                        .HasColumnType("integer");

                    b.Property<int>("Zenith")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("LocationForecastsId");

                    b.ToTable("Forecasts");
                });

            modelBuilder.Entity("SolarRadiationStore.Lib.LocationForecasts", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<double>("Latitude")
                        .HasColumnType("double precision");

                    b.Property<Point>("Location")
                        .HasColumnType("geography (point)");

                    b.Property<double>("Longitude")
                        .HasColumnType("double precision");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("timestamp without time zone");

                    b.Property<long>("Srid")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("Location")
                        .HasAnnotation("Npgsql:IndexMethod", "GIST");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("SolarRadiationStore.Lib.DbSolradForecast", b =>
                {
                    b.HasOne("SolarRadiationStore.Lib.LocationForecasts", "LocationForecasts")
                        .WithMany("Forecasts")
                        .HasForeignKey("LocationForecastsId");
                });
#pragma warning restore 612, 618
        }
    }
}
