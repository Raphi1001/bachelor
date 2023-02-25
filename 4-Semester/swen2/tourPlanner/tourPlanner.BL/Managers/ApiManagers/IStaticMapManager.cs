using tourPlanner.Models.Route;

namespace tourPlanner.BL.Managers.ApiManagers
{
    public interface IStaticMapManager
    {
        Task<string> CreateImageForRoute(RouteInternal dto);
    }
}