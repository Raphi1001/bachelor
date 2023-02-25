using Npgsql;
using tourPlanner.DAL.Configuration;
using tourPlanner.DAL.Exceptions;
using tourPlanner.Logging;

namespace tourPlanner.DAL.Repositories
{
    public abstract class Repository : IRepository
    {
        protected readonly IDatabaseConfiguration configuration;

        protected readonly ILogger _logger;

        public Repository(IDatabaseConfiguration configuration, ILogManager logManager)
        {
            this.configuration = configuration;
            _logger = logManager.GetLogger<Repository>();
        }

        protected abstract void EnsureTables();

        protected T ExecuteWithConnection<T>(Func<NpgsqlConnection, T> command)
        {
            try
            {
                using var connection = new NpgsqlConnection(configuration.ConnectionString);
                connection.Open();

                return command(connection);
            }
            catch (NpgsqlException e)
            {            
                _logger.Fatal($"Unable to connect to database. Error: [{e.Message}]");
                throw new DatabaseErrorException();
            }
        }
    }
}
