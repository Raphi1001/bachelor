using tourPlanner.Models.Enums;
using tourPlanner.Models.Exceptions;

namespace tourPlanner.Models.Route
{
    public class RouteInternal
    { 
        public string To { get; private set; }
        public string From { get; private set; }
        public Coordinates Ul { get; private set; } = new Coordinates(0, 0);
        public Coordinates Lr { get; private set; } = new Coordinates(0, 0);
        public double Distance { get; private set; }
        public RouteTypeEnum RouteType { get; private set; }
        public int PlannedDurationS { get; private set; }        
        public double PlannedDurationH { get; private set; }

        private string sessionId = "";
        public string SessionId
        {
            get { return sessionId; }
            set
            {
                sessionId = value is null ? throw new InvalidParameterException($"The Paramter To SessionId not be null") : value;

            }
        }

        // warning is not correct here 
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public RouteInternal(string? to, string? from, Coordinates ul,                      
        Coordinates lr, double? distance, RouteTypeEnum routeType, int? plannedDurationS, string? sessionId)
        {
            SetBaseRoute(to, from, distance, routeType, plannedDurationS);           
            Ul = ul;        
            Lr = lr;   
            SessionId = sessionId is null ? throw new InvalidParameterException($"The Paramter To SessionId not be null") : sessionId;
        }
        public RouteInternal(string? to, string? from, double? distance, RouteTypeEnum routeType, int? plannedDurationS)
        {
            SetBaseRoute(to, from, distance, routeType, plannedDurationS);
        }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.


        private void SetBaseRoute(string? to, string? from, double? distance, RouteTypeEnum routeType, int? plannedDurationS)
        {
            To = to is null ? throw new InvalidParameterException($"The Paramter To should not be null") : to;
            From = from is null ? throw new InvalidParameterException($"The Paramter From should not be null") : from;
            Distance = distance is null ? throw new InvalidParameterException($"The Paramter Distance should not be null") : (double)distance;
            RouteType = routeType;
            PlannedDurationS = plannedDurationS is null ? throw new InvalidParameterException($"The Paramter PlannedDurationS should not be null") : (int)plannedDurationS;
            PlannedDurationH = PlannedDurationS / 60 / 60;
            From = from is null ? throw new InvalidParameterException($"The Paramter From should not be null") : from;
        }

        public RouteTransfere ToTransfere()
        {
            return new RouteTransfere()
            {
                To = To,
                From = From,
                Ul_lat = Ul.Latitude,
                Ul_lng = Ul.Longitude,
                Lr_lat = Lr.Latitude,
                Lr_lng = Lr.Longitude,
                Distance = Distance,
                RouteType = RouteType.ToString(),
                PlannedDurationS = PlannedDurationS,
                SessionId = SessionId
            };
    }
    }
}
