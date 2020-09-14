using System;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace SolarRadiationStore.Lib
{
    public class CoordinateFactory
    {
        public CoordinateFactory(uint srid = 4326)
        {
            srid = (srid == 0) ? 4326 : srid;
            GeometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(Convert.ToInt32(srid));
        }

        public GeometryFactory GeometryFactory { get; }

        public Point CreatePoint(double x, double y) => GeometryFactory.CreatePoint(new Coordinate(x, y));
    }
}
