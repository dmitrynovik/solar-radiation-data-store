using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using SolradParserTest;

namespace SolarRadiationStore.Lib
{
    public class SolarRadiationDataContext : DbContext
    {
        private string _host = "localhost";
        private string _db = "SolarRadiationStore";
        private string _user = "admin";
        private string _password = "Password1?";

        public DbSet<LocationForecasts> Locations { get; set; }

        public SolarRadiationDataContext WithHost(string host)
        {
            _host = host;
            return this;
        }

        public SolarRadiationDataContext WithDatabase(string db)
        {
            _db = db;
            return this;
        }

        public SolarRadiationDataContext WithUser(string user)
        {
            _user = user;
            return this;
        }

        public SolarRadiationDataContext WithPassword(string password)
        {
            _password = password;
            return this;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseNpgsql($"Host={_host};Database={_db};Username={_user};Password={_password}",
                o => o.UseNetTopologySuite());
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasPostgresExtension("postgis");

            builder.Entity<LocationForecasts>().Property(c => c.Location).HasColumnType("geography (point)");
            builder.Entity<LocationForecasts>().HasIndex(c => c.Location).HasMethod("GIST")/*.IsUnique()*/;

            builder.Entity<SolradForecast>().HasKey(f => new { f.PeriodEnd, f.Period });
        }
    }

    public class LocationForecasts
    {
        static readonly GeometryFactory GeometryFactory;

        static LocationForecasts()
        {
            GeometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
        }

        public LocationForecasts() {  }

        public LocationForecasts(SolradNwpForecast forecast)
        {
            Location = GeometryFactory.CreatePoint(new Coordinate(forecast.Latitude, forecast.Longitude));
            Srid = forecast.Srid;
            Created = forecast.Created;
            Modified = forecast.Modified;
            Forecasts = forecast.Forecasts;
        }

        public long Id { get; set; }

        public Point Location { get; set; }

        /// <summary>
        /// The Spatial Reference System Identifier of the geometry (0 if unspecified).
        /// </summary>
        public uint Srid { get; set; }

        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        public List<SolradForecast> Forecasts { get; set; }
    }

}
