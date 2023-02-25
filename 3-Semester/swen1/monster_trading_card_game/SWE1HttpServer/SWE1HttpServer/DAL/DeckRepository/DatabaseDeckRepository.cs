using Npgsql;
using SWE1HttpServer.Models;
using SWE1HttpServer.Models.Logs;
using SWE1HttpServer.Models.Exceptions;
using SWE1HttpServer.Managers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SWE1HttpServer.DAL.DeckRepository
{
    class DatabaseDeckRepository : IDeckRepository
    {
        private const string CreateTableCommand = @"
CREATE TABLE IF NOT EXISTS decks( 
    username VARCHAR PRIMARY KEY  REFERENCES users(username) ON DELETE CASCADE, 
    card1 VARCHAR UNIQUE NOT NULL REFERENCES cards(id) ON DELETE CASCADE, 
    card2 VARCHAR UNIQUE NOT NULL REFERENCES cards(id) ON DELETE CASCADE, 
    card3 VARCHAR UNIQUE NOT NULL REFERENCES cards(id) ON DELETE CASCADE, 
    card4 VARCHAR UNIQUE NOT NULL REFERENCES cards(id) ON DELETE CASCADE )";

        private const string SelectDeckIdsByUserCommand = "SELECT card1, card2, card3, card4 FROM decks WHERE username=@username";
        private const string DeleteDeckCommand = "DELETE FROM decks WHERE username = @username";
        private const string InsertDeckCommand = "INSERT INTO decks(username, card1, card2, card3, card4) VALUES (@username, @0, @1, @2, @3)";
        private const string UpdateDeckByUsernameCommand = "UPDATE decks SET card1=@card1, card2=@card2 card3=@card3 card4=@card4 WHERE username=@username";

        private readonly NpgsqlConnection _connection;
        private Mutex Mutex { get; }

        public DatabaseDeckRepository(NpgsqlConnection connection, Mutex mutex)
        {
            Mutex = mutex;
            _connection = connection;
            EnsureTables();
        }

        private void EnsureTables()
        {
            using var cmd = new NpgsqlCommand(CreateTableCommand, _connection);
            cmd.ExecuteNonQuery();
        }
        private IList<string> ReadCardIds(IDataRecord record)
        {
            IList<string> deckIds = new List<string>();

            for (int i = 0; i < record.FieldCount; ++i)
            {
                deckIds.Add(record.GetString(i));
            }

            return deckIds;
        }

        public IList<string> GetUserDeckIds(string username)
        {
            IList<string> deckIds = null;

            using (var cmd = new NpgsqlCommand(SelectDeckIdsByUserCommand, _connection))
            {
                cmd.Parameters.AddWithValue("username", username);
                try
                {
                    Mutex.WaitOne();
                    using var reader = cmd.ExecuteReader();

                    // take the first row, if any
                    if (reader.Read())
                    {
                        deckIds = ReadCardIds(reader);
                    }
                }
                finally
                {
                    Mutex.ReleaseMutex();
                }
            }

            return deckIds;
        }

        public bool DeleteDeck(string username)
        {
            var affectedRows = 0;
            try
            {
                using var cmd = new NpgsqlCommand(DeleteDeckCommand, _connection);
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
            }


            return affectedRows > 0;
        }

        public bool InsertDeck(IList<string> cardIds, string username)
        {
            if (cardIds.Count != (int)Constants.deckSize)
                throw new InternalServerErrorException();

            var affectedRows = 0;
            try
            {
                using var cmd = new NpgsqlCommand(InsertDeckCommand, _connection);
                cmd.Parameters.AddWithValue("username", username);

                for (int i = 0; i < cardIds.Count; ++i)
                {
                    cmd.Parameters.AddWithValue(i.ToString(), cardIds[i]);
                }

                try
                {
                    Mutex.WaitOne();
                    affectedRows = cmd.ExecuteNonQuery();
                }
                finally { Mutex.ReleaseMutex(); }

            }
            catch (PostgresException)
            {
                // this might happen, if the package already exists (constraint violation)
                // we just catch it an keep affectedRows at zero
            }

            return affectedRows > 0;
        }

        public void UpdateDeck(IList<string> cardIds, string username)
        {
            if (cardIds.Count != (int)Constants.deckSize)
                throw new InternalServerErrorException();


            using NpgsqlCommand cmd = new NpgsqlCommand(UpdateDeckByUsernameCommand, _connection);
            cmd.Parameters.AddWithValue("card1", cardIds[0]);
            cmd.Parameters.AddWithValue("card1", cardIds[1]);
            cmd.Parameters.AddWithValue("card1", cardIds[2]);
            cmd.Parameters.AddWithValue("card1", cardIds[3]);
            try
            {
                Mutex.WaitOne();
                cmd.ExecuteNonQuery();
            }
            finally
            { Mutex.ReleaseMutex(); }
        }
    }
}
