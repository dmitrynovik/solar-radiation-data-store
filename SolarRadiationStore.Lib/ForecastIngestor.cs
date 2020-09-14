using SolradParserTest;

namespace SolarRadiationStore.Lib
{
    public class ForecastIngestor
    {
        public ulong Ingest(SolradNwpForecast ingested, SolarRadiationDataContext dbContext, ulong counter)
        {
            var location = new LocationForecasts(ingested);
            dbContext.Locations.Add(location);
            dbContext.SaveChanges();
            return counter + 1;
        }
    }
}
