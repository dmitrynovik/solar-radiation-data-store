using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SolradParserTest;

namespace SolarRadiationStore.Lib
{
    public class ForecastIngestor
    {
        public void Ingest(SolradNwpForecast ingested, SolarRadiationDataContext dbContext)
        {
            const double FLOATING_POINT_TOLERANCE = 0.1;

            var ingestedLocation = new CoordinateFactory(ingested.Srid).CreatePoint(ingested.Latitude, ingested.Longitude);

            // get locating from the database:
            var location = dbContext.Locations
                .Include(f => f.Forecasts)
                    .FirstOrDefault(f =>
                        f.Location.Distance(ingestedLocation) <= FLOATING_POINT_TOLERANCE);

            if (location == null)
            {
                // location does not exist => create
                location = new LocationForecasts(ingested);
                dbContext.Locations.Add(location);
            }
            else
            {
                // location exists => update if newer:
                if (location.Modified < ingested.Modified)
                {
                    // update location:
                    location.Srid = ingested.Srid;
                    location.Created = ingested.Created;
                    location.Modified = ingested.Modified;
                    location.Forecasts = ingested.Forecasts.Select(f => new DbSolradForecast(location, f)).ToList();

                    // update forecasts. Here assume that the forecast key is a combination of { PeriodEnd, Period }
                    //var savedForecasts = MakeForecastDictionary(location.Forecasts);
                    //var ingestedForecasts = MakeForecastDictionary(ingested.Forecasts);
                    //foreach (var ingestedForecast in ingestedForecasts)
                    //{
                    //    if (!savedForecasts.TryGetValue(ingestedForecast.Key, out var savedForecast))
                    //    {
                    //        // a new forecast => add:
                    //        location.Forecasts.Add(new DbSolradForecast(location, ingestedForecast.Value));
                    //    }
                    //    else
                    //    {
                    //        UpdateExistingForecast(savedForecast, ingestedForecast);
                    //    }
                    //}
                }
            }

            dbContext.SaveChanges();
        }

        private static void UpdateExistingForecast(SolradForecast savedForecast, KeyValuePair<string, SolradForecast> ingestedForecast)
        {
            savedForecast.Ghi = ingestedForecast.Value.Ghi;
            savedForecast.Ghi90 = ingestedForecast.Value.Ghi90;
            savedForecast.Ghi10 = ingestedForecast.Value.Ghi10;
            savedForecast.ClearSkyGhi = ingestedForecast.Value.ClearSkyGhi;
            savedForecast.ClearSkyDni = ingestedForecast.Value.ClearSkyDni;
            savedForecast.ClearSkyDhi = ingestedForecast.Value.ClearSkyDhi;
            savedForecast.Ebh = ingestedForecast.Value.Ebh;
            savedForecast.Ebh10 = ingestedForecast.Value.Ebh10;
            savedForecast.Ebh90 = ingestedForecast.Value.Ebh90;
            savedForecast.Dni = ingestedForecast.Value.Dni;
            savedForecast.Dni10 = ingestedForecast.Value.Dni10;
            savedForecast.Dni90 = ingestedForecast.Value.Dni90;
            savedForecast.AirTemp = ingestedForecast.Value.AirTemp;
            savedForecast.Zenith = ingestedForecast.Value.Zenith;
            savedForecast.Azimuth = ingestedForecast.Value.Azimuth;
            savedForecast.CloudOpacity = ingestedForecast.Value.CloudOpacity;
            savedForecast.SnowClearnessRooftop = ingestedForecast.Value.SnowClearnessRooftop;
            savedForecast.SnowClearnessUtility = ingestedForecast.Value.SnowClearnessUtility;
        }

        /// <summary>Creates a forecasts dictionary for fast in-memory access with the key {PeriodEnd, Period}</summary>
        private static IDictionary<string, SolradForecast>
            MakeForecastDictionary(IEnumerable<SolradForecast> forecasts) =>
            forecasts.ToDictionary(f => $"{f.PeriodEnd.Ticks}-{f.Period.Ticks}", f => f);
    }
}
