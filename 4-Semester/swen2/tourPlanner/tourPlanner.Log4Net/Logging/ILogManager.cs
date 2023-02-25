
namespace tourPlanner.Logging
{
    public interface ILogManager
    {
        public ILogger GetLogger<TContext>();
    }
}
