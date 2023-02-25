using Npgsql;
using System.Data;
using tourPlanner.Logging;
using tourPlanner.Models.TourLog;
using tourPlanner.DAL.Repositories;
using tourPlanner.DAL.Configuration;
using tourPlanner.DAL.Exceptions;

namespace tourPlanner.DAL.TourLogRepository
{
    public class DBTourLogRepository : Repository, ITourLogRepository
    {
        private const string CreateTableCommand = @"CREATE TABLE IF NOT EXISTS tour_logs 
                                                    (
                                                        log_id          uuid        primary key, 
                                                        tour_id         uuid        references tours ON DELETE CASCADE,
                                                        rating          integer, 
                                                        difficulty      integer, 
                                                        creation_date   date, 
                                                        time_taken_s    integer, 
                                                        tour_comment    varchar(255)
                                                    );";

        private const string DeleteTourLogCommand       = "DELETE FROM tour_logs WHERE log_id=@log_id";
        private const string InsertTourLogCommand       = "INSERT INTO tour_logs(log_id, tour_id, rating, difficulty, time_taken_s, tour_comment, creation_date) VALUES (@log_id, @tour_id, @rating, @difficulty, @time_taken_s, @tour_comment, @creation_date) ON CONFLICT (log_id) DO NOTHING";
        private const string UpdateTourLogCommand       = "UPDATE tour_logs SET rating=@rating, difficulty=@difficulty, time_taken_s=@time_taken_s, tour_comment=@tour_comment WHERE log_id=@log_id";
        private const string SelectTourLogsCommand      = "SELECT log_id, tour_id, rating, difficulty, creation_date, time_taken_s, tour_comment FROM tour_logs WHERE tour_id=@tour_id";
        private const string SelectAllTourLogsCommand   = "SELECT log_id, tour_id, rating, difficulty, creation_date, time_taken_s, tour_comment FROM tour_logs";


        public DBTourLogRepository(IDatabaseConfiguration configuration, ILogManager logManager) : base(configuration, logManager)
        {
            EnsureTables();
        }

        protected override void EnsureTables()
        {
            ExecuteWithConnection(connection =>
            {
                using var cmd = new NpgsqlCommand(CreateTableCommand, connection);

                _logger.Debug("TourLog-Table ensured");
                return cmd.ExecuteNonQuery();
            });
            
            /*
            //create admin acccount
            User admin = new User("admin", "istrator");
            InsertUser(admin);  
            */
        }

        public IEnumerable<TourLogTransfere> GetAllTourLogs()
        {
            return ExecuteWithConnection(connection =>
            {
                IEnumerable<TourLogTransfere> tourLogs = new List<TourLogTransfere>();
                using (var cmd = new NpgsqlCommand(SelectAllTourLogsCommand, connection))
                {
                    // take all rows, if any
                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        TourLogTransfere currentTourLog = ReadTourLog(reader);
                        tourLogs = tourLogs.Append(currentTourLog);
                    }
                }
                return tourLogs;
            });
        }
        
        public IEnumerable<TourLogTransfere> GetTourLogs(Guid tourId)
        {    
            return ExecuteWithConnection(connection =>
            {
                IEnumerable<TourLogTransfere> tourLogs = new List<TourLogTransfere>();
                using (var cmd = new NpgsqlCommand(SelectTourLogsCommand, connection))
                {
                    cmd.Parameters.AddWithValue("tour_id", tourId);

                    // take all rows, if any
                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        TourLogTransfere currentTourLog = ReadTourLog(reader);
                        tourLogs = tourLogs.Append(currentTourLog);
                    }
                }
                return tourLogs;
            });
        }
        public bool InsertTourLog(TourLogInternal tourLog)
        {
            return ExecuteWithConnection(connection =>
            {
                using var cmd = new NpgsqlCommand(InsertTourLogCommand, connection);
                cmd.Parameters.AddWithValue("log_id", tourLog.Id);
                cmd.Parameters.AddWithValue("tour_id", tourLog.TourId);
                cmd.Parameters.AddWithValue("rating", (int)tourLog.TourRating);
                cmd.Parameters.AddWithValue("difficulty", (int)tourLog.TourDifficulty);
                cmd.Parameters.AddWithValue("time_taken_s", tourLog.TimeTakenS);
                cmd.Parameters.AddWithValue("creation_date", tourLog.CreationDate);
                cmd.Parameters.AddWithValue("tour_comment", tourLog.TourComment);

                var result = cmd.ExecuteNonQuery();
                _logger.Debug($"TourLog [{tourLog.Id}] was added to TourLog-Table");
                return result > 0;
                });
        }
        public void DeleteTourLog(Guid id)
        {
            ExecuteWithConnection(connection =>
            {
                using var cmd = new NpgsqlCommand(DeleteTourLogCommand, connection);
                cmd.Parameters.AddWithValue("log_id", id);
                _logger.Debug($"TourLog [{id}] was removed from TourLog-Table");
                return cmd.ExecuteNonQuery();
            });
        }
        public bool UpdateTourLog(TourLogInternal tourLog)
        {
            return ExecuteWithConnection(connection =>
            {
                using var cmd = new NpgsqlCommand(UpdateTourLogCommand, connection);
                cmd.Parameters.AddWithValue("rating", (int)tourLog.TourRating);
                cmd.Parameters.AddWithValue("difficulty", (int)tourLog.TourDifficulty);
                cmd.Parameters.AddWithValue("time_taken_s", tourLog.TimeTakenS);
                cmd.Parameters.AddWithValue("tour_comment", tourLog.TourComment);
                cmd.Parameters.AddWithValue("log_id", tourLog.Id);


                var result = cmd.ExecuteNonQuery();

                return result > 0;
            });
        }

        private TourLogTransfere ReadTourLog(IDataRecord record)
        {
            try
            {
                return new TourLogTransfere() 
                {
#pragma warning disable CS8604 // Possible null reference argument.

                    Id = string.IsNullOrEmpty(record["log_id"]?.ToString()) ? null : Guid.Parse(record["log_id"].ToString()), /* warning is wrong here */
                    TourId = string.IsNullOrEmpty(record["tour_id"]?.ToString()) ? null : Guid.Parse(record["tour_id"].ToString()), /* warning is wrong here */
#pragma warning restore CS8604 // Possible null reference argument.
                    TourRating = record["rating"].ToString(), 
                    TourDifficulty = record["difficulty"].ToString(), 
                    CreationDate = DateOnly.FromDateTime(DateTime.Parse(record["creation_date"].ToString())),
                    TimeTakenS = Convert.ToInt32(record["time_taken_s"]), 
                    TourComment = record["tour_comment"].ToString()
                };
            }
            catch (Exception e)
            {
                _logger.Error($"Database Error. ErrMsg: [{e.Message}]");
                throw new DatabaseErrorException(); 
            }
        }

    }
}