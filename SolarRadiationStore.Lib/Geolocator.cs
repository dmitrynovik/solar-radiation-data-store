using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SolarRadiationStore.Lib
{
    public class GeoLocator
    {
        private readonly SolarRadiationDataContext _dataContext;
        private readonly CoordinateFactory _coordinateFactory;

        public GeoLocator(SolarRadiationDataContext dataContext, uint srid = 0)
        {
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
            _coordinateFactory = new CoordinateFactory(srid);
        }

        public LocationForecasts FindNearestLocation(double latitude, double longitude)
        {
            var point = _coordinateFactory.CreatePoint(latitude, longitude);
            return _dataContext.Locations
                .Include(l => l.Forecasts)
                .OrderBy(loc => loc.Location.Distance(point))
                .FirstOrDefault();
        }
    }
}
