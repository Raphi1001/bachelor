using Npgsql;
using SWE1HttpServer.Models;
using SWE1HttpServer.Models.Exceptions;
using SWE1HttpServer.Models.Logs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SWE1HttpServer.DAL.UserRepository
{
    class DatabaseUserRepository : IUserRepository
    {
        private const string CreateTableCommand = "CREATE TABLE IF NOT EXISTS users (username VARCHAR PRIMARY KEY, password VARCHAR, name VARCHAR, bio VARCHAR, image VARCHAR, coins INT, elo_score VARCHAR, played_games INT, won_games INT, lost_games INT, token VARCHAR)";

        private const string InsertUserCommand = "INSERT INTO users(username, password, name, bio, image, coins, elo_score, played_games, won_games, lost_games, token) VALUES (@username, @password, @name, @bio, @image,  @coins, @elo_score, @played_games, @won_games, @lost_games, @token)";

        private const string SelectUserByTokenCommand = "SELECT username, password FROM users WHERE token=@token";
        private const string SelectUserByCredentialsCommand = "SELECT username, password FROM users WHERE username=@username AND password=@password";

        private const string SelectUseDataCommand = "SELECT username, password, name, bio, image, coins, elo_score, played_games, won_games, lost_games FROM users WHERE token=@token";
        private const string UpdateUserDataCommand = "UPDATE users SET name=@name, bio=@bio, image=@image WHERE token=@token";

        private const string UpdateUserCoinsCommand = "UPDATE users SET coins=@coins WHERE username=@username";
        private const string UpdateUserScoreCommand = "UPDATE users SET elo_score=@elo_score, played_games=@played_games, won_games=@won_games, lost_games=@lost_games WHERE username=@username";

        private const string SelectAllUserStatsCommand = "SELECT elo_score FROM users ORDER BY elo_score DESC";


        private readonly NpgsqlConnection _connection;
        private Mutex Mutex;

        public DatabaseUserRepository(NpgsqlConnection connection, Mutex mutex)
        {
            Mutex = mutex;
            _connection = connection;
            EnsureTables();
        }

        private void EnsureTables()
        {
            //Create user table
            using var cmd = new NpgsqlCommand(CreateTableCommand, _connection);
            cmd.ExecuteNonQuery();

            /*
            //create admin acccount
            User admin = new User("admin", "istrator");
            InsertUser(admin);  
            */
        }

        public User GetUserByAuthToken(string authToken)
        {
            User user = null;
            using (var cmd = new NpgsqlCommand(SelectUserByTokenCommand, _connection))
            {
                cmd.Parameters.AddWithValue("token", authToken);

                try
                {
                    Mutex.WaitOne();
                    using var reader = cmd.ExecuteReader();
                    // take the first row, if any
                    if (reader.Read())
                    {
                        user = ReadUserCredentials(reader);
                    }
                }
                finally
                {

                    Mutex.ReleaseMutex();
                }

            }
            return user;
        }

        public User GetUserByCredentials(string username, string password)
        {
            User user = null;
            using (var cmd = new NpgsqlCommand(SelectUserByCredentialsCommand, _connection))
            {
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("password", password);

                try
                {
                    Mutex.WaitOne();
                    using var reader = cmd.ExecuteReader();
                    // take the first row, if any
                    if (reader.Read())
                    {
                        user = ReadUserCredentials(reader);
                    }
                }
                finally
                {

                    Mutex.ReleaseMutex();
                }
            }
            return user;
        }

        public User GetUserByName(string username)
        {
            return null;
        }

        public User GetUserData(string authToken)
        {
            User user = null;
            try
            {
                using (var cmd = new NpgsqlCommand(SelectUseDataCommand, _connection))
                {
                    cmd.Parameters.AddWithValue("token", authToken);

                    try
                    {
                        Mutex.WaitOne();
                        using var reader = cmd.ExecuteReader();
                        // take the first row, if any
                        if (reader.Read())
                        {
                            user = ReadUserData(reader);
                        }
                    }
                    finally
                    {

                        Mutex.ReleaseMutex();
                    }
                }
            }
            catch (PostgresException)
            {
            }


            return user;
        }



        public bool InsertUser(User user)
        {
            var affectedRows = 0;
            try
            {
                using var cmd = new NpgsqlCommand(InsertUserCommand, _connection);
                cmd.Parameters.AddWithValue("username", user.Username);
                cmd.Parameters.AddWithValue("password", user.Password);
                cmd.Parameters.AddWithValue("name", user.Name);
                cmd.Parameters.AddWithValue("bio", user.Bio);
                cmd.Parameters.AddWithValue("image", user.Image);
                cmd.Parameters.AddWithValue("coins", user.Coins);
                cmd.Parameters.AddWithValue("elo_score", user.EloScore);
                cmd.Parameters.AddWithValue("played_games", user.PlayedGames);
                cmd.Parameters.AddWithValue("won_games", user.WonGames);
                cmd.Parameters.AddWithValue("lost_games", user.LostGames);
                cmd.Parameters.AddWithValue("token", user.Token);

                try
                {
                    Mutex.WaitOne();
                    affectedRows = cmd.ExecuteNonQuery();
                }
                finally { Mutex.ReleaseMutex(); }
            }

            catch (PostgresException)
            {
                throw new InvalidException();
            }

            return affectedRows > 0;
        }

        public bool UpdateUserData(string token, UserDataLog userDataCredentials)
        {
            var affectedRows = 0;
            try
            {
                using var cmd = new NpgsqlCommand(UpdateUserDataCommand, _connection);
                cmd.Parameters.AddWithValue("token", token);
                cmd.Parameters.AddWithValue("name", userDataCredentials.Name);
                cmd.Parameters.AddWithValue("bio", userDataCredentials.Bio);
                cmd.Parameters.AddWithValue("image", userDataCredentials.Image);
                try
                {
                    Mutex.WaitOne();
                    affectedRows = cmd.ExecuteNonQuery();
                }
                finally { Mutex.ReleaseMutex(); }
            }
            catch (PostgresException)
            {
                // this might happen, if the card already exists (constraint violation)
                // we just catch it and keep affectedRows at zero
            }

            return affectedRows > 0;
        }


        public bool UpdateUserCoins(string username, int coins)
        {
            var affectedRows = 0;
            try
            {
                using var cmd = new NpgsqlCommand(UpdateUserCoinsCommand, _connection);
                cmd.Parameters.AddWithValue("coins", coins);
                cmd.Parameters.AddWithValue("username", username);
                try
                {
                    Mutex.WaitOne();
                    affectedRows = cmd.ExecuteNonQuery();
                }
                finally { Mutex.ReleaseMutex(); }

            }
            catch (PostgresException)
            {
                // this might happen, if the card already exists (constraint violation)
                // we just catch it and keep affectedRows at zero
            }

            return affectedRows > 0;
        }
        public bool UpdateUserScore(string username, int elo, int playedGames, int wonGames, int lostGames)
        {
            var affectedRows = 0;
            try
            {
                using var cmd = new NpgsqlCommand(UpdateUserScoreCommand, _connection);
                cmd.Parameters.AddWithValue("elo_score", elo);
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("played_games", playedGames);
                cmd.Parameters.AddWithValue("lost_games", lostGames);
                cmd.Parameters.AddWithValue("won_games", wonGames);

                try
                {
                    Mutex.WaitOne();
                    affectedRows = cmd.ExecuteNonQuery();
                }
                finally { Mutex.ReleaseMutex(); }

            }
            catch (PostgresException)
            {
                // this might happen, if the card already exists (constraint violation)
                // we just catch it and keep affectedRows at zero
            }

            return affectedRows > 0;
        }

        private User ReadUserCredentials(IDataRecord record)
        {
            User user = new User(record["username"].ToString(), record["password"].ToString());
            return user;
        }

        private User ReadUserData(IDataRecord record)
        {
            User user = new User(record["username"].ToString(), record["password"].ToString(), Convert.ToInt32(record["coins"]), Convert.ToInt32(record["elo_score"]), Convert.ToInt32(record["played_games"]), Convert.ToInt32(record["won_games"]), Convert.ToInt32(record["lost_games"]));
            user.SetUserData(record["name"].ToString(), record["bio"].ToString(), record["image"].ToString());
            return user;
        }

        public IEnumerable<int> GetAllUserStats()
        {
            IList<int> scoreboard = new List<int>();
            using (var cmd = new NpgsqlCommand(SelectAllUserStatsCommand, _connection))
            {
                try
                {
                    Mutex.WaitOne();
                    using var reader = cmd.ExecuteReader();

                    // take all rows, if any
                    while (reader.Read())
                    {
                        scoreboard.Add(Convert.ToInt32(reader["elo_score"]));
                    }
                }
                finally { Mutex.ReleaseMutex(); }
            }
            return scoreboard;
        }
    }
}
