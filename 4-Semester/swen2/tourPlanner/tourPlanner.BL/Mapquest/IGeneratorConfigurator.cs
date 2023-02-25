
namespace tourPlanner.BL.Mapquest
{
    public interface IGeneratorConfigurator
    {
        string ApiKey { get; }
        string ImagePath { get; }
        string ImageBaseUrl { get; }
        string DirectionsBaseUrl { get; }
    }
}
