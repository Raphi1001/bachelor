
namespace tourPlanner.Models.MapQuest
{
    public class MqResponse
    {
        public MqRoute? Route { get; set; }
        public MqInfo? Info { get; set; }
    }

    public class MqRoute
    {
        public IList<MqLocation>? Locations { get; set; }
        public MqBoundingBox? BoundingBox { get; set; }
        public double? Distance { get; set; }
        public string? SessionId { get; set; }
        public MqOptions? Options { get; set; }
        public int Time { get; set; }
    }

    public class MqInfo
    {
        public int? Statuscode { get; set; }
        public IEnumerable<string>? Messages { get; set; }
    }

    public class MqLocation
    {
        public string? AdminArea4 { get; set; } // County
        public string? AdminArea5 { get; set; } // City
        public string? AdminArea1 { get; set; } // Country
        public string? AdminArea3 { get; set; } // State          
    }

    public class MqBoundingBox
    {
        public MqCoordinates? Lr { get; set; }
        public MqCoordinates? Ul { get; set; }
    }

    public class MqCoordinates
    {
        public double? Lng { get; set; }
        public double? Lat { get; set; }
    }

    public class MqOptions
    {
        public string? RouteType { get; set; }
    }
}