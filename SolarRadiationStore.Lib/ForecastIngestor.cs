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
    }
}
