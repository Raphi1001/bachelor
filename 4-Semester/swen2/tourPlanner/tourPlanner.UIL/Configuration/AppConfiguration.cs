using Microsoft.Extensions.Configuration;
using tourPlanner.BL.Mapquest;
using tourPlanner.DAL.Configuration;

namespace tourPlanner.UIL.Configuration
{
    internal class AppConfiguration : IDatabaseConfiguration, IGeneratorConfigurator, IDirectoryConfiguration
    {
        private readonly IConfiguration configuration;
        
        /* DATABASE */
        public string ConnectionString => configuration["database:connectionstring"];

        /* MAPQUEST API */
        public string ApiKey => configuration["mapquest:key"];
        public string ImageBaseUrl => configuration["mqstaticmapapi:baseurl"];
        public string DirectionsBaseUrl => configuration["mqdirectionsapi:baseurl"];

        /* DIRECTORY */
        public string ImagePath => configuration["mqstaticmapapi:imagePath"];
        public string FileDirectory => configuration["filedirectory:directory"];

        public AppConfiguration(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
    }
}
