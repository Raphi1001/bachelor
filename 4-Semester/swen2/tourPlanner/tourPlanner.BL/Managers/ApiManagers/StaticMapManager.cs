using tourPlanner.BL.Mapquest;
using tourPlanner.DAL.Mapquest;
using tourPlanner.Models.Route;

namespace tourPlanner.BL.Managers.ApiManagers
{
    public class StaticMapManager : IStaticMapManager
    {
        private readonly IImageDAO _imgSaver;
        private readonly IStaticMapGenerator _imgGenerator;

        public StaticMapManager(IStaticMapGenerator imgGenerator, IImageDAO imgSaver)
        {
            _imgGenerator = imgGenerator;
            _imgSaver = imgSaver;
        }

        public async Task<string> CreateImageForRoute(RouteInternal dto)
        {
            var byteStream = await _imgGenerator.GenerateImageForRoute(dto);
            var relativeImgPath = _imgSaver.SaveImage(byteStream);
            
            var dir = Directory.GetCurrentDirectory();
            var absoluteImgPath = Path.Combine(dir, relativeImgPath);
            return absoluteImgPath; // can't be null
        }
    }
}
