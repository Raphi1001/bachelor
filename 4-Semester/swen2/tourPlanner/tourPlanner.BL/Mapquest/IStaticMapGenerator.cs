using tourPlanner.Models.Route;

namespace tourPlanner.BL.Mapquest
{
    public interface IStaticMapGenerator
    {
        Task<byte[]?> GenerateImageForRoute(RouteInternal dto);
    }
}