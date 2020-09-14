using System;
using System.Collections.Generic;
using SolradParserTest;

namespace SolarRadiationStore.Lib
{
    public class ForecastIngestor
    {
        public ulong IngestSingle(SolradNwpForecast ingested, SolarRadiationDataContext dbContext, ulong count, bool saveChanges = true)
        {
            var location = new LocationForecasts(ingested);
            dbContext.Locations.Add(location);
            if (saveChanges)
            {
                dbContext.SaveChanges();
            }
            return count + 1;
        }

        public void IngestAll(SolarRadiationDataContext dbContext, IEnumerable<SolradNwpForecast> parsedForecastData, Action<Exception> onError = null)
        {
            const int SAVE_BATCH_SIZE = 100;

            ulong count = 0;

            // This speeds up bulk inserts:
            dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

            foreach (var forecast in parsedForecastData)
            {
                try
                {
                    count = IngestSingle(forecast, dbContext, count, false);
                    if (count % SAVE_BATCH_SIZE == 0)
                    {
                        SaveBatch(dbContext, count, onError);
                    }
                }
                catch (Exception e)
                {
                    onError?.Invoke(e);
                }
            }

            // Save last batch:
            SaveBatch(dbContext, count, onError);
        }

        private static void SaveBatch(SolarRadiationDataContext dbContext, ulong count, Action<Exception> onError)
        {
            try
            {
                dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
                dbContext.SaveChanges();
                Console.WriteLine($"Processed {count} rows");
            }
            catch (Exception e)
            {
                onError?.Invoke(e);
            }
            dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
        }


        //private static void UpdateExistingForecast(SolradForecast savedForecast, KeyValuePair<string, SolradForecast> ingestedForecast)
        //{
        //    savedForecast.Ghi = ingestedForecast.Value.Ghi;
        //    savedForecast.Ghi90 = ingestedForecast.Value.Ghi90;
        //    savedForecast.Ghi10 = ingestedForecast.Value.Ghi10;
        //    savedForecast.ClearSkyGhi = ingestedForecast.Value.ClearSkyGhi;
        //    savedForecast.ClearSkyDni = ingestedForecast.Value.ClearSkyDni;
        //    savedForecast.ClearSkyDhi = ingestedForecast.Value.ClearSkyDhi;
        //    savedForecast.Ebh = ingestedForecast.Value.Ebh;
        //    savedForecast.Ebh10 = ingestedForecast.Value.Ebh10;
        //    savedForecast.Ebh90 = ingestedForecast.Value.Ebh90;
        //    savedForecast.Dni = ingestedForecast.Value.Dni;
        //    savedForecast.Dni10 = ingestedForecast.Value.Dni10;
        //    savedForecast.Dni90 = ingestedForecast.Value.Dni90;
        //    savedForecast.AirTemp = ingestedForecast.Value.AirTemp;
        //    savedForecast.Zenith = ingestedForecast.Value.Zenith;
        //    savedForecast.Azimuth = ingestedForecast.Value.Azimuth;
        //    savedForecast.CloudOpacity = ingestedForecast.Value.CloudOpacity;
        //    savedForecast.SnowClearnessRooftop = ingestedForecast.Value.SnowClearnessRooftop;
        //    savedForecast.SnowClearnessUtility = ingestedForecast.Value.SnowClearnessUtility;
        //}

        ///// <summary>Creates a forecasts dictionary for fast in-memory access with the key {PeriodEnd, Period}</summary>
        //private static IDictionary<string, SolradForecast>
        //    MakeForecastDictionary(IEnumerable<SolradForecast> forecasts) =>
        //    forecasts.ToDictionary(f => $"{f.PeriodEnd.Ticks}-{f.Period.Ticks}", f => f);
    }
}
