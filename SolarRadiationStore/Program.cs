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

            Console.WriteLine(parsedForecastData);

            // Do stuff with data..

            using var dbContext = new SolarRadiationDataContext()
                .WithHost("localhost")
                .WithDatabase("SolarRadiationStore")
                .WithUser("admin")
                .WithPassword("Password1?");

            var forecastIngestor = new ForecastIngestor();
            ulong count = 0;
            foreach (var forecast in parsedForecastData)
            {
                try
                {
                    forecastIngestor.Ingest(forecast, dbContext);
                    Console.WriteLine($"Processed {++count} rows");
                }
                catch (Exception e)
                {
                    LogError(e);
                }
            }
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
