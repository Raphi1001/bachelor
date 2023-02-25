using tourPlanner.Models.Route;
using tourPlanner.Models.Enums;

namespace tourPlanner.BL.Managers.ApiManagers
{
    public interface IRouteManager
    {
        Task<RouteInternal?> CreateRouteAsync(string to, string from, RouteTypeEnum routeType);
    }
}