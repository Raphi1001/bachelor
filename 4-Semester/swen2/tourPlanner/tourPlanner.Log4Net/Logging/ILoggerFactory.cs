
namespace tourPlanner.Logging
{
    public interface ILoggerFactory
    {
        ILogger CreateLogger<TContext>();
    }
}
