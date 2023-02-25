using Npgsql;
using SWE1HttpServer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1HttpServer.DAL.EnumRepository
{
    class DatabaseEnumRepository : IEnumRepository
    {        
        private const string CreateCardTypeEnumCommand = "CREATE TYPE card_type AS ENUM ('monster', 'spell');";
        private const string CreateElementTypeEnumCommand = "CREATE TYPE element_type AS ENUM ('regular', 'fire', 'water');";

        private const string DuplicateExceptionString = "DO $$ BEGIN  EXCEPTION WHEN duplicate_object THEN null; END $$;";

        private readonly NpgsqlConnection _connection;

        public DatabaseEnumRepository(NpgsqlConnection connection)
        {
            _connection = connection;
            EnsureTable(CreateCardTypeEnumCommand);
            EnsureTable(CreateElementTypeEnumCommand);
        }

        private void EnsureTable(string CreateCommand)
        {
            string command = DuplicateExceptionString.Insert(12, CreateCommand);
            using var cmd = new NpgsqlCommand(command, _connection);
            cmd.ExecuteNonQuery();
        }
    }
}
