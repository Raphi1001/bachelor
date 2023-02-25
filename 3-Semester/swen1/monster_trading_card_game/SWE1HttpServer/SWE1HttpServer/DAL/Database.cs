using SWE1HttpServer.DAL.UserRepository;
using SWE1HttpServer.DAL.CardRepository;
using SWE1HttpServer.DAL.PackageRepository;
using SWE1HttpServer.DAL.DeckRepository;
using SWE1HttpServer.DAL.EnumRepository;
using SWE1HttpServer.Models.Exceptions;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWE1HttpServer.DAL.TradeRepository;
using System.Threading;

namespace SWE1HttpServer.DAL
{
    class Database
    {
        private readonly NpgsqlConnection _connection;
        public IUserRepository UserRepository { get; private set; }
        public ICardRepository CardRepository { get; private set; }
        public IDeckRepository DeckRepository { get; private set; }
        public IPackageRepository PackageRepository { get; private set; }
        public ITradeRepository TradeRepository { get; private set; }

        private static Mutex Mutex = new Mutex();
        public Database(string connectionString)
        {
            try
            {
                _connection = new NpgsqlConnection(connectionString);
                _connection.Open();

                // first enums, then users, then cards, ...
                // we need this special order because of dependencies

                new DatabaseEnumRepository(_connection);
                UserRepository = new DatabaseUserRepository(_connection, Mutex);
                CardRepository = new DatabaseCardRepository(_connection, Mutex);
                DeckRepository = new DatabaseDeckRepository(_connection, Mutex);
                PackageRepository = new DatabasePackageRepository(_connection, Mutex);
                TradeRepository = new DatabaseTradeRepository(_connection, Mutex);
            }
            catch (NpgsqlException e)
            {
                // provide our own custom exception
                throw new DataAccessFailedException("Could not connect to or initialize database", e);
            }
        }
    }
}
