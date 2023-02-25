using tourPlanner.Logging;
using tourPlanner.Models.Route;
using tourPlanner.BL.Exceptions;

namespace tourPlanner.BL.Mapquest
{
    public class StaticMapGenerator : IStaticMapGenerator
    {
        private readonly ILogger _logger;
        private static HttpClient? _staticmapapi;
        private readonly IGeneratorConfigurator _apiConfig;

        public StaticMapGenerator(IGeneratorConfigurator apiConfig,
            ILogManager logManager)
        {
            _apiConfig = apiConfig;

            _logger = logManager.GetLogger<StaticMapGenerator>();

            _staticmapapi = new HttpClient();
            _staticmapapi.BaseAddress = new Uri(apiConfig.DirectionsBaseUrl);
        }

        public async Task<byte[]?> GenerateImageForRoute(RouteInternal dto)
        {
            byte[]? resStream = default;

            try
            {
                Uri uri = new Uri(BuildEndpoint(dto));

                if (_staticmapapi is null)
                    throw new ServerErrorException();

                var res = await _staticmapapi.GetAsync(uri);
                resStream = await res.Content.ReadAsByteArrayAsync();

                if (resStream is null)
                    throw new ErrorImageRequest("Image byte stream couldn't be retrieved from api.");
            }
            catch (Exception e)
            {
                _logger.Error($"Static Map Request went wrong. ErrMsg: [{e.Message}]");
            }

            return resStream;
        }

        private string BuildEndpoint(RouteInternal dto)
        {
            if (dto.SessionId is null)
                throw new ServerErrorException();

            string endpoint = _apiConfig.ImageBaseUrl;

            if (!endpoint.EndsWith("?")) // BUG: Url ends with ? in appjson.config but ImageBaseUrl doesn't
                endpoint += "?";

            return endpoint += AssembleRequestParameters(dto);
        }

        private string AssembleRequestParameters(RouteInternal dto)
        {
            return $"key={_apiConfig.ApiKey}&session={dto.SessionId}&boundingBox=" +
                $"{ChangeCoordinateFormat(dto.Ul.Latitude.ToString())}," +
                $"{ChangeCoordinateFormat(dto.Ul.Longitude.ToString())}," +
                $"{ChangeCoordinateFormat(dto.Lr.Latitude.ToString())}," +
                $"{ChangeCoordinateFormat(dto.Lr.Longitude.ToString())}";
        }

        private string ChangeCoordinateFormat(string coo)
        {
            return coo.Replace(",", ".");
        }
    }
}
