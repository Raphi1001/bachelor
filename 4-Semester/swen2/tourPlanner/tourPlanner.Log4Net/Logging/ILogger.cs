
namespace tourPlanner.Logging
{
    public interface ILogger
    {
        void Warn(string message);
        void Debug(string message);
        void Error(string message);
        void Fatal(string message);
    }
}
