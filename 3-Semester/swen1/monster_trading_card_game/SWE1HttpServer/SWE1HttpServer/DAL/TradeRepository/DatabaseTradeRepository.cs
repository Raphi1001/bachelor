using SWE1HttpServer.Models.Exceptions;
using SWE1HttpServer.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWE1HttpServer.Models.Logs;
using SWE1HttpServer.Managers;
using SWE1HttpServer.Managers.Trades;
using System.Threading;

namespace SWE1HttpServer.DAL.TradeRepository
{
    class DatabaseTradeRepository : ITradeRepository
    {
        private const string CreateTableCommand = @"
CREATE TABLE IF NOT EXISTS trades(  
    trade_id VARCHAR PRIMARY KEY,
    card_id VARCHAR UNIQUE NOT NULL REFERENCES cards(id) ON DELETE CASCADE,
    required_card_type card_type NOT NULL,
    required_min_damage INT NOT NULL)";

        private const string SelectTradeByIdCommand = "SELECT trade_id, card_id, required_card_type, required_min_damage FROM trades WHERE trade_id=@trade_id";
        private const string SelectTradeByCardIdCommand = "SELECT trade_id, card_id, required_card_type, required_min_damage FROM trades WHERE card_id=@card_id";

        private const string SelectAllTradesCommand = "SELECT trade_id, card_id, required_card_type, required_min_damage FROM trades";

        private const string InsertTradeCommand = "INSERT INTO trades(trade_id, card_id, required_card_type, required_min_damage) VALUES (@trade_id, @card_id, @required_card_type, @required_min_damage)";

        private const string DeletePackageCommand = "DELETE FROM trades WHERE trade_id = @trade_id";

        private readonly NpgsqlConnection _connection;
        private Mutex Mutex { get; }


        public DatabaseTradeRepository(NpgsqlConnection connection, Mutex mutex)
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

        public Trade GetTradeById(string tradeId)
        {
            Trade currentTrade = null;

            using (var cmd = new NpgsqlCommand(SelectTradeByIdCommand, _connection))
            {
                cmd.Parameters.AddWithValue("trade_id", tradeId);
                try
                {
                    Mutex.WaitOne();
                    using var reader = cmd.ExecuteReader();

                    // take the fist rows, if any
                    if (reader.Read())
                    {
                        currentTrade = ReadTrade(reader);
                    }
                }
                finally { Mutex.ReleaseMutex(); }

            }

            return currentTrade;
        }


        public Trade GetTradeByCardId(string cardId)
        {
            Trade currentTrade = null;

            using (var cmd = new NpgsqlCommand(SelectTradeByCardIdCommand, _connection))
            {
                cmd.Parameters.AddWithValue("card_id", cardId);
                try
                {
                    Mutex.WaitOne();
                    using var reader = cmd.ExecuteReader();

                    // take the fist rows, if any
                    if (reader.Read())
                    {
                        currentTrade = ReadTrade(reader);
                    }
                }
                finally
                { Mutex.ReleaseMutex(); }
            }

            return currentTrade;
        }

        public IEnumerable<Trade> GetAllTradeOffers()
        {
            IEnumerable<Trade> trades = new List<Trade>();

            using (var cmd = new NpgsqlCommand(SelectAllTradesCommand, _connection))
            {
                try
                {
                    Mutex.WaitOne();
                    using var reader = cmd.ExecuteReader();

                    // take all rows, if any
                    while (reader.Read())
                    {
                        Trade currentTrade = ReadTrade(reader);
                        trades = trades.Append(currentTrade);
                    }
                }
                finally { Mutex.ReleaseMutex(); }
            }

            return trades;
        }

        private static Trade ReadTrade(IDataRecord record)
        {
            try
            {
                return TradeManager.CreateTrade(record["trade_id"].ToString(), record["card_id"].ToString(), record["required_card_type"].ToString(), Convert.ToInt32(record["required_min_damage"]));
            }
            catch (InvalidException)
            {
                throw new InternalServerErrorException();
            }
        }

        public bool InsertTrade(Trade trade)
        {
            var affectedRows = 0;
            try
            {
                using var cmd = new NpgsqlCommand(InsertTradeCommand, _connection);
                cmd.Parameters.AddWithValue("trade_id", trade.Id);
                cmd.Parameters.AddWithValue("card_id", trade.CardId);
                cmd.Parameters.AddWithValue("required_card_type", trade.RequiredCardType);
                cmd.Parameters.AddWithValue("required_min_damage", trade.RequiredMinimumDamage);

                try
                {
                    Mutex.WaitOne();
                    affectedRows = cmd.ExecuteNonQuery();
                }
                finally { Mutex.ReleaseMutex(); }
            }
            catch (PostgresException)
            {
                // this might happen, if the trade already exists (constraint violation)
            }

            return affectedRows > 0;
        }

        public bool DeleteTrade(string tradeId)
        {
            var affectedRows = 0;
            try
            {
                using var cmd = new NpgsqlCommand(DeletePackageCommand, _connection);
                cmd.Parameters.AddWithValue("trade_id", tradeId);
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
    }
}
