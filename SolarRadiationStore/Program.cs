using System;
using System.IO;
using System.Linq;
using SolarRadiationStore.Lib;
using SolradParserTest;

namespace SolarRadiationStore
{
    class Program
    {
        static void Main(string[] args)
        {
            var dataDirectory = new DirectoryInfo("Data");
            var allPropertyData = dataDirectory.CollectSiteChunkDataFromDirectory(
                SolradForecastChunkParser.DailyDataActions.Keys.ToList(), "aust");
            var chunkParser = new SolradForecastChunkParser();

            var parsedForecastData = chunkParser.PopulatePointsDaily<SolradNwpForecast>
                (allPropertyData, Console.WriteLine);

            // Do stuff with data..

            using var dbContext = new SolarRadiationDataContext()
                .WithHost("localhost")
                .WithDatabase("SolarRadiationStore")
                .WithUser("admin")
                .WithPassword("Password1?");

            const int SAVE_BATCH_SIZE = 100;

            var forecastIngestor = new ForecastIngestor();
            ulong count = 0;

            // This speeds up bulk inserts:
            dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

            foreach (var forecast in parsedForecastData)
            {
                try
                {
                    count = forecastIngestor.Ingest(forecast, dbContext, count);
                    if (count % SAVE_BATCH_SIZE == 0)
                    {
                        SaveBatch(dbContext, count);
                    }
                }
                catch (Exception e)
                {
                    LogError(e);
                }
            }

            // Save last batch:
            SaveBatch(dbContext, count);
        }

        private static void SaveBatch(SolarRadiationDataContext dbContext, ulong count)
        {
            try
            {
                dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
                dbContext.SaveChanges();
                Console.WriteLine($"Processed {count} rows");
            }
            catch (Exception e)
            {
                LogError(e);
            }
            dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
        }

        private static void LogError(Exception e)
        {
            var consoleColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e);
            Console.ForegroundColor = consoleColor;
        }
    }
}
