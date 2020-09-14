using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace SolradParserTest
{
    public class SolradForecastChunkParser
    {
        private List<DateTime> _timesSeries;
        private string _allLats;
        private string _allLngs;

        private const string Format = "dd-MMM-yyyy HH:mm:ss";

        public static readonly Dictionary<string, Action<string, SolradForecast>> DailyDataActions =
            new Dictionary<string, Action<string, SolradForecast>>
            {
                {"Azi", (val, solradForecast) => { solradForecast.Azimuth = int.Parse(val); }},
                {"Cldns", (val, solradForecast) => { solradForecast.CloudOpacity = int.Parse(val); }},
                {"Csk", (val, solradForecast) => { solradForecast.ClearSkyGhi = int.Parse(val); }},
                {"CskEbn", (val, solradForecast) => { solradForecast.ClearSkyDni = int.Parse(val); }},
                {"CskEdh", (val, solradForecast) => { solradForecast.ClearSkyDhi = int.Parse(val); }},
                {"EbhHi", (val, solradForecast) => { solradForecast.Ebh90 = int.Parse(val); }},
                {"EbhLo", (val, solradForecast) => { solradForecast.Ebh10 = int.Parse(val); }},
                {"EbhMid", (val, solradForecast) => { solradForecast.Ebh = int.Parse(val); }},
                {"EbnHi", (val, solradForecast) => { solradForecast.Dni90 = int.Parse(val); }},
                {"EbnLo", (val, solradForecast) => { solradForecast.Dni10 = int.Parse(val); }},
                {"EbnMid", (val, solradForecast) => { solradForecast.Dni = int.Parse(val); }},
                {"EdhHi", (val, solradForecast) => { solradForecast.Dhi90 = int.Parse(val); }},
                {"EdhLo", (val, solradForecast) => { solradForecast.Dhi10 = int.Parse(val); }},
                {"EdhMid", (val, solradForecast) => { solradForecast.Dhi = int.Parse(val); }},
                {"GhiHi", (val, solradForecast) => { solradForecast.Ghi90 = int.Parse(val); }},
                {"GhiLo", (val, solradForecast) => { solradForecast.Ghi10 = int.Parse(val); }},
                {"GhiMid", (val, solradForecast) => { solradForecast.Ghi = int.Parse(val); }},
                {"SnowClrRoof", (val, solradForecast) => { solradForecast.SnowClearnessRooftop = int.Parse(val); }},
                {"SnowClrUtility", (val, solradForecast) => { solradForecast.SnowClearnessUtility = int.Parse(val); }},
                {"Tmp", (val, solradForecast) => { solradForecast.AirTemp = int.Parse(val); }},
                {"Zen", (val, solradForecast) => { solradForecast.Zenith = int.Parse(val); }}
            };


        public static readonly Dictionary<string, Action<string, SolradForecast>> SatDataActions =
            new Dictionary<string, Action<string, SolradForecast>>
            {
                {"Cldns", (val, solradForecast) => { solradForecast.CloudOpacity = int.Parse(val); }},
                {"EbhHi", (val, solradForecast) => { solradForecast.Ebh90 = int.Parse(val); }},
                {"EbhLo", (val, solradForecast) => { solradForecast.Ebh10 = int.Parse(val); }},
                {"EbhMid", (val, solradForecast) => { solradForecast.Ebh = int.Parse(val); }},
                {"EbnHi", (val, solradForecast) => { solradForecast.Dni90 = int.Parse(val); }},
                {"EbnLo", (val, solradForecast) => { solradForecast.Dni10 = int.Parse(val); }},
                {"EbnMid", (val, solradForecast) => { solradForecast.Dni = int.Parse(val); }},
                {"EdhHi", (val, solradForecast) => { solradForecast.Dhi90 = int.Parse(val); }},
                {"EdhLo", (val, solradForecast) => { solradForecast.Dhi10 = int.Parse(val); }},
                {"EdhMid", (val, solradForecast) => { solradForecast.Dhi = int.Parse(val); }},
                {"GhiHi", (val, solradForecast) => { solradForecast.Ghi90 = int.Parse(val); }},
                {"GhiLo", (val, solradForecast) => { solradForecast.Ghi10 = int.Parse(val); }},
                {"GhiMid", (val, solradForecast) => { solradForecast.Ghi = int.Parse(val); }}
            };

        public static readonly Dictionary<string, Action<string, SolradForecast>> HistoricForecastsDataActions =
            new Dictionary<string, Action<string, SolradForecast>>
            {
                {"Azi", (val, solradForecast) => { solradForecast.Azimuth = int.Parse(val); }},
                {"Cldns", (val, solradForecast) => { solradForecast.CloudOpacity = int.Parse(val); }},
                {"EbhMid", (val, solradForecast) => { solradForecast.Ebh = int.Parse(val); }},
                {"EbnMid", (val, solradForecast) => { solradForecast.Dni = int.Parse(val); }},
                {"EdhMid", (val, solradForecast) => { solradForecast.Dhi = int.Parse(val); }},
                {"GhiMid", (val, solradForecast) => { solradForecast.Ghi = int.Parse(val); }},
                {"SnowClrRoof", (val, solradForecast) => { solradForecast.SnowClearnessRooftop = int.Parse(val); }},
                {"SnowClrUtility", (val, solradForecast) => { solradForecast.SnowClearnessUtility = int.Parse(val); }},
                {"Tmp", (val, solradForecast) => { solradForecast.AirTemp = int.Parse(val); }},
                {"Zen", (val, solradForecast) => { solradForecast.Zenith = int.Parse(val); }}
            };

        public List<T> PopulatePointsSat<T>(Dictionary<string, List<string>> allChunkData, Action<string> logMsg)
            where T : SolradNwpForecast
        {
            var result = new List<T>();

            foreach (var data in allChunkData)
            {
                result = IterateForecasts(data.Value, result, SatDataActions[data.Key], logMsg);
            }

            return result;
        }

        public List<T> PopulatePointsHistoricForecasts<T>(Dictionary<string, List<string>> allChunkData,
            Action<string> logMsg) where T : SolradNwpForecast
        {
            var result = new List<T>();

            foreach (var data in allChunkData)
            {
                result = IterateForecasts(data.Value, result, HistoricForecastsDataActions[data.Key], logMsg);
            }

            return result;
        }


        public List<T> PopulatePointsDaily<T>(Dictionary<string, List<string>> allPropertyData, Action<string> logMsg)
            where T : SolradNwpForecast
        {
            var result = new List<T>();

            foreach (var data in allPropertyData)
            {
                result = IterateForecasts(data.Value, result, DailyDataActions[data.Key], logMsg);
            }

            return result;
        }

        private List<T> IterateForecasts<T>(List<string> allLines, List<T> result,
            Action<string, SolradForecast> parseAndAssign, Action<string> logMsg) where T : SolradNwpForecast
        {
            // numerOfTimeSeries is calculated by simply removing the 2 known header lines of the CSV, eg Lat/Lng locations
            if (result == null || result.Count == 0)
                result = InitData<T>(allLines, allLines.Count - 2, logMsg);
            // Val eg, "Zen"
            for (var timeIndex = 2; timeIndex < allLines.Count; timeIndex++)
            {
                var currentLocationValues = allLines[timeIndex].Split(',');

                for (var pointIndex = 0; pointIndex < result.Count; pointIndex++)
                {
                    var solradGridPoint = result[pointIndex];
                    parseAndAssign(currentLocationValues[pointIndex + 1], solradGridPoint.Forecasts[timeIndex - 2]);
                }
            }

            return result;
        }


        private List<T> InitData<T>(List<string> allLines, int numerOfTimeSeries, Action<string> logMsg)
            where T : SolradNwpForecast
        {
            var result = new List<T>();
            numerOfTimeSeries = numerOfTimeSeries == 0 ? 469 : numerOfTimeSeries;
            _allLats = allLines[0];
            _allLngs = allLines[1];
            _timesSeries = new List<DateTime>(numerOfTimeSeries);

            var periodDuration = TimeSpan.FromMinutes(30);
            if (_timesSeries.Count > 1)
                periodDuration = _timesSeries[1] - _timesSeries[0];

            for (var i = 2; i < allLines.Count; i++)
            {
                var time = allLines[i].Split(',')[0];
                time = time.Contains(":") ? time : time.Trim() + " 00:00:00";
                _timesSeries.Add(DateTime.ParseExact(time, Format, CultureInfo.InvariantCulture));
            }

            var lats = new List<double>(_allLats.Split(',').ToList().Skip(1).Select(double.Parse));
            var lngs = new List<double>(_allLngs.Split(',').ToList().Skip(1).Select(double.Parse));
            logMsg("Locations found: " + lats.Count);

            for (var index = 0; index < lats.Count; index++)
            {
                var lat = lats[index];
                var lng = lngs[index];
                var solradPoint = Activator.CreateInstance<T>();
                solradPoint.Latitude = lat;
                solradPoint.Longitude = lng;
                solradPoint.Srid = 4326;
                solradPoint.Forecasts = new List<SolradForecast>(numerOfTimeSeries);
                for (var i = 0; i < numerOfTimeSeries; i++)
                {
                    solradPoint.Forecasts.Add(new SolradForecast
                    {
                        PeriodEnd = _timesSeries[i],
                        Period = periodDuration
                    });
                }

                result.Add(solradPoint);
            }

            logMsg("Initialized data with " + result.Count + " entries.");
            return result;
        }
    }
    
    public static class SolradParserUtils
    {
        public static Dictionary<string, List<string>> CollectSiteChunkDataFromDirectory(
            this DirectoryInfo dirInfo, List<string> propertiesToParse, string satelliteDomain)
        {
            var files = dirInfo.GetFiles("*.csv")
                .Select(x => x.FullName)
                .ToList();
            var result = new Dictionary<string,List<string>>();
            foreach (var propName in propertiesToParse)
            {
                var file = files.FirstOrDefault(x => x.EndsWith($"site{propName}_{satelliteDomain}_chunk.csv"));
                if (file == null)
                {
                    throw new Exception($"Missing file for {propName}");
                }
                result.Add(propName, File.ReadLines(file).ToList());
            }

            return result;
        }
    }
    
    public class SolradForecast
    {
        public int Ghi { get; set; }
        public int Ghi90 { get; set; }
        public int Ghi10 { get; set; }
        public int ClearSkyGhi { get; set; }
        public int ClearSkyDni { get; set; }
        public int ClearSkyDhi { get; set; }
        public int Ebh { get; set; }
        public int Ebh10 { get; set; }
        public int Ebh90 { get; set; }
        public int Dni { get; set; }
        public int Dni10 { get; set; }
        public int Dni90 { get; set; }
        public int Dhi { get; set; }
        public int Dhi10 { get; set; }
        public int Dhi90 { get; set; }
        public int AirTemp { get; set; }
        public int Zenith { get; set; }
        public int Azimuth { get; set; }
        /// <summary>
        /// Cloud opacity between 0-100 cloud cover.
        /// Where 100 is extremely cloudy and 0 is clear.
        /// </summary>
        public int CloudOpacity { get; set; }
        
        /// <summary>
        /// Values between 0-100 to represent the clearness of the snow cover.
        /// Where 100 is clear and 0 is completely covered.
        /// </summary>
        public int? SnowClearnessRooftop { get; set; }
        /// <summary>
        /// Values between 0-100 to represent the clearness of the snow cover.
        /// Where 100 is clear and 0 is completely covered.
        /// </summary>
        public int? SnowClearnessUtility { get; set; }

        public DateTime PeriodEnd { get; set; }
        public TimeSpan Period { get; set; }
    }
    
    public class SolradNwpForecast
    {
        public long Id { get; set; }

        //public GeoPoint Location { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        
        /// <summary>
        /// The Spatial Reference System Identifier of the geometry (0 if unspecified).
        /// </summary>
        public uint Srid { get; set; }

        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        public List<SolradForecast> Forecasts { get; set; }
    }
}