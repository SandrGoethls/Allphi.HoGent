using Newtonsoft.Json;

namespace AllPhi.HoGent.Blazor.Extensions
{
        public class LocationModel
        {
        public class BoundingBox
        {
            [JsonProperty("LowerLeft")]
            public LowerLeft LowerLeft { get; set; }

            [JsonProperty("UpperRight")]
            public UpperRight UpperRight { get; set; }
        }

        public class Location
        {
            [JsonProperty("Lat_WGS84")]
            public double LatWGS84 { get; set; }

            [JsonProperty("Lon_WGS84")]
            public double LonWGS84 { get; set; }

            [JsonProperty("X_Lambert72")]
            public double XLambert72 { get; set; }

            [JsonProperty("Y_Lambert72")]
            public double YLambert72 { get; set; }
        }

        public class LocationResult
        {
            [JsonProperty("Municipality")]
            public string Municipality { get; set; }

            [JsonProperty("Zipcode")]
            public string Zipcode { get; set; }

            [JsonProperty("Thoroughfarename")]
            public string Thoroughfarename { get; set; }

            [JsonProperty("Housenumber")]
            public object Housenumber { get; set; }

            [JsonProperty("ID")]
            public int ID { get; set; }

            [JsonProperty("FormattedAddress")]
            public string FormattedAddress { get; set; }

            [JsonProperty("Location")]
            public Location Location { get; set; }

            [JsonProperty("LocationType")]
            public string LocationType { get; set; }

            [JsonProperty("BoundingBox")]
            public BoundingBox BoundingBox { get; set; }
        }

        public class LowerLeft
        {
            [JsonProperty("Lat_WGS84")]
            public double LatWGS84 { get; set; }

            [JsonProperty("Lon_WGS84")]
            public double LonWGS84 { get; set; }

            [JsonProperty("X_Lambert72")]
            public double XLambert72 { get; set; }

            [JsonProperty("Y_Lambert72")]
            public double YLambert72 { get; set; }
        }

        public class Root
        {
            [JsonProperty("LocationResult")]
            public List<LocationResult> LocationResult { get; set; }
        }

        public class UpperRight
        {
            [JsonProperty("Lat_WGS84")]
            public double LatWGS84 { get; set; }

            [JsonProperty("Lon_WGS84")]
            public double LonWGS84 { get; set; }

            [JsonProperty("X_Lambert72")]
            public double XLambert72 { get; set; }

            [JsonProperty("Y_Lambert72")]
            public double YLambert72 { get; set; }
        }

    }
}
