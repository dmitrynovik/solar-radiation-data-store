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
            
            //using (var dbContext = new SolarRadiationDataContext()
            //    .WithHost("localhost")
            //    .WithDatabase("SolarRadiationStore")
            //    .WithUser("admin")
            //    .WithPassword("Password1?")
            //)
            //{

            //}
        }
    }
}
