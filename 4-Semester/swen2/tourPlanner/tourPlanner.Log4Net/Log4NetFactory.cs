using tourPlanner.Logging;

namespace tourPlanner.Log4Net
{
    public class Log4NetFactory : ILoggerFactory
    {
        private readonly string _configPath;

        public Log4NetFactory()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var basePath = currentDirectory.Split(new string[] { "\\bin" }, StringSplitOptions.None)[0]; // root of project

            _configPath = $"{basePath}\\log4net.config";
        }

        public ILogger CreateLogger<TContext>()
        {
            if (!File.Exists(_configPath))
            {
                throw new ArgumentException("Does not exist.", nameof(_configPath));
            }

            log4net.Config.XmlConfigurator.Configure(new FileInfo(_configPath));
            var logger = log4net.LogManager.GetLogger(typeof(TContext));

            return new Log4NetLogger(logger);
        }
    }
}