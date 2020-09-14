using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolarRadiationStore.Lib;
using SolradParserTest;

namespace SolarRadiationStore.WebApi.Controllers
{
    [ApiController]
    [Route("forecast")]
    public class ForecastController : ControllerBase
    {
        private readonly ILogger<ForecastController> _logger;

        public ForecastController(ILogger<ForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("nearest")]
        public SolradNwpForecast GetNearestForecasts(int latitude, int longitude)
        {

            using var dbContext = new SolarRadiationDataContext().WithEnabledDebugging(); // print generated SQL to Debug output

            var locator = new GeoLocator(dbContext);

            return locator.FindNearestLocation(latitude, longitude)?.ToSolradNwpForecast();
        }
    }
}
