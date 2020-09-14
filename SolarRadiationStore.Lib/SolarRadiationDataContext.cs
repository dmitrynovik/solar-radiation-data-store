using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private bool _enableDebugLogging;

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

        public SolarRadiationDataContext WithEnabledDebugging()
        {
            _enableDebugLogging = true;
            return this;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (_enableDebugLogging)
            {
                builder.UseLoggerFactory(new LoggerFactory(new[]
                    {
                        new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider()
                    }));
            }

            builder
                .UseNpgsql($"Host={_host};Database={_db};Username={_user};Password={_password}",
                o => o.UseNetTopologySuite());
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasPostgresExtension("postgis");

            builder.Entity<LocationForecasts>().Property(c => c.Location).HasColumnType("geography (point)");
            builder.Entity<LocationForecasts>().HasIndex(c => c.Location).HasMethod("GIST")/*.IsUnique()*/;

            builder.Entity<DbSolradForecast>().ToTable("Forecasts");
            //builder.Entity<DbSolradForecast>().HasKey(f => new { f.LocationForecastId, f.PeriodEnd, f.Period });
        }
    }

    public class LocationForecasts
    {
        public LocationForecasts() {  }

        public LocationForecasts(SolradNwpForecast forecast)
        {
            var coordinateFactory = new CoordinateFactory(forecast.Srid);
            Location = coordinateFactory.CreatePoint(forecast.Latitude, forecast.Longitude);

            Latitude = forecast.Latitude;
            Longitude = forecast.Longitude;
            Srid = forecast.Srid;
            Created = forecast.Created;
            Modified = forecast.Modified;
            Forecasts = forecast.Forecasts.Select(f => new DbSolradForecast(this, f)).ToList();
        }

        public SolradNwpForecast ToSolradNwpForecast()
        {
            return new SolradNwpForecast
            {
                Created = Created,
                Modified = Modified,
                Latitude = Latitude,
                Longitude = Longitude,
                Srid = Srid,
                Forecasts = Forecasts.Select(f => f.ToSolradForecast()).ToList()
            };
        }

        public long Id { get; set; }

        public Point Location { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        /// <summary>The Spatial Reference System Identifier of the geometry (0 if unspecified).</summary>
        public uint Srid { get; set; }

        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        public List<DbSolradForecast> Forecasts { get; set; }
    }

    public class DbSolradForecast : SolradForecast
    {
        public DbSolradForecast() {  }

        public DbSolradForecast(LocationForecasts l, SolradForecast f)
        {
            LocationForecasts = l;

            Ghi = f.Ghi;
            Ghi90 = f.Ghi90;
            Ghi10 = f.Ghi10;
            ClearSkyGhi = f.ClearSkyGhi;
            ClearSkyDni = f.ClearSkyDni;
            ClearSkyDhi = f.ClearSkyDhi;
            Ebh = f.Ebh;
            Ebh10 = f.Ebh10;
            Ebh90 = f.Ebh90;
            Dni = f.Dni;
            Dni10 = f.Dni10;
            Dni90 = f.Dni90;
            AirTemp = f.AirTemp;
            Zenith = f.Zenith;
            Azimuth = f.Azimuth;
            CloudOpacity = f.CloudOpacity;
            SnowClearnessRooftop = f.SnowClearnessRooftop;
            SnowClearnessUtility = f.SnowClearnessUtility;
            Period = f.Period;
            PeriodEnd = f.PeriodEnd;
        }

        public long Id { get; set; }

        public int LocationForecastId { get; set; }
        public LocationForecasts LocationForecasts { get; set; }

        public SolradForecast ToSolradForecast()
        {
            return new SolradForecast
            {
                Ghi = Ghi,
                Ghi90 = Ghi90,
                Ghi10 = Ghi10,
                ClearSkyGhi = ClearSkyGhi,
                ClearSkyDni = ClearSkyDni,
                ClearSkyDhi = ClearSkyDhi,
                Ebh = Ebh,
                Ebh10 = Ebh10,
                Ebh90 = Ebh90,
                Dni = Dni,
                Dni10 = Dni10,
                Dni90 = Dni90,
                AirTemp = AirTemp,
                Zenith = Zenith,
                Azimuth = Azimuth,
                CloudOpacity = CloudOpacity,
                SnowClearnessRooftop = SnowClearnessRooftop,
                SnowClearnessUtility = SnowClearnessUtility,
                Period = Period,
                PeriodEnd = PeriodEnd,
            };
        }
    }
}
