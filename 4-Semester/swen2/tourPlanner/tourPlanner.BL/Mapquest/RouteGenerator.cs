using System.Net.Http.Json;
using tourPlanner.Models.Route;
using tourPlanner.Models.MapQuest;
using tourPlanner.BL.Exceptions;
using tourPlanner.Logging;
using tourPlanner.Models.Enums;

namespace tourPlanner.BL.Mapquest
{
    public class RouteGenerator : IRouteGenerator
    {
        private readonly ILogger _logger;
        private readonly HttpClient _directionsapi;
        private readonly IGeneratorConfigurator _apiConfig;

        public RouteGenerator(IGeneratorConfigurator apiConfig, HttpClient directionsapi, ILogManager logManager)
        {
            _apiConfig = apiConfig;

            _logger = logManager.GetLogger<RouteGenerator>();

            _directionsapi = directionsapi;
            _directionsapi.BaseAddress = new Uri(_apiConfig.DirectionsBaseUrl);
            _directionsapi.Timeout = TimeSpan.FromSeconds(7);
        }

        public async Task<RouteTransfere?> GenerateRouteAsync(string to, string from, RouteTypeEnum routeType)
        {
            RouteTransfere? routeTransfere = null;

            try
            {
                Uri uri = new Uri(BuildEndpoint(to, from, routeType));

                MqResponse? res = await _directionsapi.GetFromJsonAsync<MqResponse>(uri);

                if (res?.Info?.Messages?.Count() <= 0) // is a valid route
                {
                    routeTransfere = new RouteTransfere(
                        AssembleAddress(res?.Route?.Locations?[1]), // to
                        AssembleAddress(res?.Route?.Locations?[0]), // from
                        res?.Route?.BoundingBox?.Ul?.Lng,
                        res?.Route?.BoundingBox?.Ul?.Lat,
                        res?.Route?.BoundingBox?.Lr?.Lng,
                        res?.Route?.BoundingBox?.Lr?.Lat,
                        res?.Route?.Distance,
                        res?.Route?.Options?.RouteType,
                        res?.Route?.Time,
                        res?.Route?.SessionId);

                    _logger.Debug($"Fetched Routeinfo: from[{AssembleAddress(res?.Route?.Locations?[0])}], to[{AssembleAddress(res?.Route?.Locations?[1])}], sessionId[{res?.Route?.SessionId}]");
                }
                else
                {
                    /* Logging happens directly bc of res access */
                    _logger.Error($"TourPlanner.BL.Mapquest - Error in route retrievel: " +
                        $"From: [{from}] , To: [{to}] failed.");
                    res?.Info?.Messages?.ToList().ForEach(i => _logger.Error($"Api Error Message: {i}"));

                    throw new InvalidRouteException(res?.Info?.Messages?.ToString());
                }
            }
            catch (OperationCanceledException)
            {
                throw new TimeOutException("The Api was not able to respond in time.");
            }

            return routeTransfere;
        }

        private string BuildEndpoint(string to, string from, RouteTypeEnum routeType)
        {         
            string endpoint = _apiConfig.DirectionsBaseUrl;
            endpoint += $"key={_apiConfig.ApiKey}&from={from}&to={to}&routeType={routeType}&unit=k";

            return endpoint;
        }

        private static string AssembleAddress(MqLocation? loc)
        {
            //only use set areas in output string
            string output = "";

            output = CheckAdminAreaNullOrEmpty(loc?.AdminArea5, output); // City
            output = CheckAdminAreaNullOrEmpty(loc?.AdminArea4, output); // County
            output = CheckAdminAreaNullOrEmpty(loc?.AdminArea3, output); // State
            output = CheckAdminAreaNullOrEmpty(loc?.AdminArea1, output); // Country

            return output;
        }

        private static string CheckAdminAreaNullOrEmpty(string? str, string output)
        {
            if (!string.IsNullOrEmpty(str))
            {
                if (output.Equals(""))
                    return str;

                return $"{output}, {str}";
            }

            return output;
        }
    }
}
