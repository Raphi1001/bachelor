
namespace tourPlanner.Logging
{
    public class LogManager : ILogManager
    {
        public LogManager(ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
        }

        public ILoggerFactory LoggerFactory { get; set; }

        public ILogger GetLogger<TContext>()
        {
            return LoggerFactory.CreateLogger<TContext>();
        }
    }
}
