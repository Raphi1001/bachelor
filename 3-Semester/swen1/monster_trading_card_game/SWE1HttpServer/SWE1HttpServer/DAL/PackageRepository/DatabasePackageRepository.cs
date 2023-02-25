using SWE1HttpServer.Models.Exceptions;
using SWE1HttpServer.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SWE1HttpServer.DAL.PackageRepository
{
    class DatabasePackageRepository : IPackageRepository
    {
        private const string CreateTableCommand = @"
CREATE TABLE IF NOT EXISTS packages(  
    card1 VARCHAR UNIQUE NOT NULL REFERENCES cards(id) ON DELETE CASCADE,
    card2 VARCHAR UNIQUE NOT NULL REFERENCES cards(id) ON DELETE CASCADE, 
    card3 VARCHAR UNIQUE NOT NULL REFERENCES cards(id) ON DELETE CASCADE, 
    card4 VARCHAR UNIQUE NOT NULL REFERENCES cards(id) ON DELETE CASCADE, 
    card5 VARCHAR UNIQUE NOT NULL REFERENCES cards(id) ON DELETE CASCADE )";

        private const string InsertPackageCommand = @"
INSERT INTO packages(card1, card2, card3, card4, card5) VALUES (@0, @1, @2, @3, @4)";

        private const string SelectRandomPackageCommand = @"
SELECT * FROM packages OFFSET floor(random() * (SELECT COUNT(*) FROM packages)) LIMIT 1";
        private const string SelectFirstPackageCommand = @"	
SELECT * FROM packages LIMIT 1;";
        private const string DeletePackageCommand = @"
DELETE FROM packages WHERE card1 = @card1";

        private readonly NpgsqlConnection _connection;
        private Mutex Mutex { get; }
        public DatabasePackageRepository(NpgsqlConnection connection, Mutex mutex)
        {
            Mutex = mutex;
            _connection = connection;
            EnsureTables();
        }

        public bool InsertPackage(IList<Card> package)
        {
            if (package.Count != (int)Constants.packageSize)
                throw new InternalServerErrorException();
            var affectedRows = 0;
            try
            {
                using var cmd = new NpgsqlCommand(InsertPackageCommand, _connection);
                for (int i = 0; i < package.Count; ++i)
                {
                    cmd.Parameters.AddWithValue(i.ToString(), package[i].Id);
                }

                Mutex.WaitOne();
                affectedRows = cmd.ExecuteNonQuery();
            }
            catch (PostgresException)
            {
                // this might happen, if the package already exists (constraint violation)
                throw new ConflictException();
            }
            finally { Mutex.ReleaseMutex(); }

            return affectedRows > 0;
        }

        public IList<string> GetRandomPackage(User user)
        {

            IList<string> package = null;
            using (var cmd = new NpgsqlCommand(SelectRandomPackageCommand, _connection))
            {
                try
                {


                    Mutex.WaitOne();

                    using var reader = cmd.ExecuteReader();


                    // take the first row, if any
                    if (reader.Read())
                    {
                        package = ReadPackage(reader);
                    }
                }
                finally { Mutex.ReleaseMutex(); }


            }
            return package;
        }

        public IList<string> GetFirstPackage(User user)
        {

            IList<string> package = null;
            using (var cmd = new NpgsqlCommand(SelectFirstPackageCommand, _connection))
            {
                try
                {
                    Mutex.WaitOne();
                    using var reader = cmd.ExecuteReader();

                    // take the first row, if any
                    if (reader.Read())
                    {
                        package = ReadPackage(reader);
                    }
                }
                finally
                {
                    Mutex.ReleaseMutex();

                }
            }
            return package;
        }

        private void EnsureTables()
        {
            using var cmd = new NpgsqlCommand(CreateTableCommand, _connection);
            cmd.ExecuteNonQuery();
        }

        private IList<string> ReadPackage(IDataRecord record)
        {
            IList<string> package = new List<string>();
            for (int i = 0; i < record.FieldCount; ++i)
            {
                package.Add(record.GetString(i));

            }
            return package;
        }

        public bool DeletePackage(string card1)
        {
            var affectedRows = 0;
            try
            {
                using var cmd = new NpgsqlCommand(DeletePackageCommand, _connection);
                cmd.Parameters.AddWithValue("card1", card1);

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
