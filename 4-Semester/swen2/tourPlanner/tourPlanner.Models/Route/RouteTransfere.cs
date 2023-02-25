using tourPlanner.Models.Enums;
using tourPlanner.Models.Exceptions;
using tourPlanner.Models.Route;

namespace tourPlanner.Models.Route
{
    public class RouteTransfere
    {
        public string? To { get; set; }
        public string? From { get; set; }
        public double? Ul_lat { get; set; }
        public double? Ul_lng { get; set; }
        public double? Lr_lat { get; set; }
        public double? Lr_lng { get; set; }
        public double? Distance { get; set; }
        public string? RouteType { get; set; }

        private int? plannedDurationS;
        public int? PlannedDurationS
        {
            get { return plannedDurationS; }
            set
            {
                plannedDurationS = value;
                PlannedDurationH = plannedDurationS / 60 / 60;
            }
        }
        public double? PlannedDurationH { get; private set; }
        public string? SessionId { get; set; } = "";
        public RouteTransfere()
        {
        }
        public RouteTransfere(string to, string from, string routeType)
        {
            To = to;
            From = from;
            RouteType = routeType;
        }
        public RouteTransfere(string? to, string? from, double? distance, string? routeType, int? plannedDurationS, double? ul_lng, double? ul_lat,
            double? lr_lng, double? lr_lat)
        {
            SetBaseRoute(to, from, distance, routeType, plannedDurationS, ul_lng, ul_lat, lr_lng, lr_lat);
        }
        public RouteTransfere(string? to, string? from, double? ul_lng, double? ul_lat,
            double? lr_lng, double? lr_lat, double? distance, string? routeType, int? plannedDurationS,
            string? sessionId)
        {
            SetBaseRoute(to, from, distance, routeType, plannedDurationS, ul_lng, ul_lat, lr_lng, lr_lat);
            SessionId = sessionId;
        }

        private void SetBaseRoute(string? to, string? from, double? distance, string? routeType, int? plannedDurationS, double? ul_lng, double? ul_lat,
            double? lr_lng, double? lr_lat)
        {
            To = to;
            From = from;
            Distance = distance;
            RouteType = routeType;
            PlannedDurationS = plannedDurationS;
            Ul_lat = ul_lat;
            Ul_lng = ul_lng;
            Lr_lat = lr_lat;
            Lr_lng = lr_lng;
        }

        public RouteInternal ToInternal()
        {
            if(SessionId is null)
            {
                return new RouteInternal(To, From, Distance, ConvertRouteType(RouteType), PlannedDurationS);
            }
            
            return new RouteInternal(To, From, ConvertToCoordinates(Ul_lat, Ul_lng), ConvertToCoordinates(Lr_lat, Lr_lng), Distance, ConvertRouteType(RouteType), PlannedDurationS, SessionId);
        }

        private static Coordinates ConvertToCoordinates(double? lng, double? lat)
        {
            if (lng is null)
            {
                throw new InvalidParameterException("The Paramter lng should not be null");
            }

            if (lat is null)
            {
                throw new InvalidParameterException("The Paramter lat should not be null");
            }

            return new Coordinates((double)lng, (double)lat);
        }

        private static RouteTypeEnum ConvertRouteType(string ?routeType)
        {
            if(routeType is null)
            {         
                throw new InvalidParameterException("The Paramter routeType should not be null");
            }

            if (!Enum.TryParse(routeType.ToLower(), out RouteTypeEnum routeTypeE))
            {
                throw new InvalidRouteTypeException($"The Paramter { routeType } is no valid rating");
            }

            return routeTypeE;
        }
    }
}