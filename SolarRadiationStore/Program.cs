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
                .WithPassword("Password1?")
                //.WithEnabledDebugging()
                ;

             new ForecastIngestor().IngestAll(dbContext, parsedForecastData, LogError, 1, true);
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
