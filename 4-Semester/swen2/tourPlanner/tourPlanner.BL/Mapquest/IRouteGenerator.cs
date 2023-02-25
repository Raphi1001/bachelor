using tourPlanner.Models.Enums;
using tourPlanner.Models.Route;

namespace tourPlanner.BL
{
    public interface IRouteGenerator
    {
        Task<RouteTransfere?> GenerateRouteAsync(string to, string from, RouteTypeEnum routeType);
    }
}
