using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;

namespace Chinook.Model.Data
{
    public class Runner : IRunner
    {
        public Runner(string connectionString)
        {
            ConnectionString = connectionString;
        }
        public string ConnectionString { get; set; }

        public IEnumerable<dynamic> ExecuteDynamic(string command, params object[] args)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                var cmd = BuildCommand(command,connection,args);
                connection.Open();
                var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (reader.Read())
                {
                    yield return reader.DynamicList();
                }
            }
        }

        public dynamic ExecuteToSingleDynamic(string command, params object[] args)
        {
            return ExecuteDynamic(command, args).FirstOrDefault();
        }
        public T ExecuteToSingle<T>(string command, params object[] args) where T : new()
        {
            return Execute<T>(command, args).FirstOrDefault();
        }
        public IEnumerable<T> Execute<T>(string sqlCommand, params object[] args) where T : new()
        {
            var reader = OpenReader(sqlCommand, args);
            while (reader.Read())
            {
                yield return reader.ToSingle<T>();
            }
            reader.Dispose();
        }


        public NpgsqlDataReader OpenReader(string sql, params object[] args)
        {
            var connection = new NpgsqlConnection(ConnectionString);
            var cmd = BuildCommand(sql,connection,args);
            connection.Open();
            var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return reader;
        }

        public async Task<NpgsqlDataReader> OpenReaderAsync(string sql, params object[] args)
        {
            var connection = new NpgsqlConnection(ConnectionString);
            var cmd = BuildCommand(sql, connection, args);
            await connection.OpenAsync();
            return await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection) as NpgsqlDataReader;
        }
        public NpgsqlCommand BuildCommand(string sql, params object[] args)
        {
            var cmd = new NpgsqlCommand(sql);
            if (args == null) return cmd;
            foreach (var arg in args)
            {
                cmd.AddParameter(arg);
            }
            return cmd;
        }
        public NpgsqlCommand BuildCommand(string sql,NpgsqlConnection connection, params object[] args)
        {
            var cmd = new NpgsqlCommand(sql) {Connection = connection};
            if (args == null) return cmd;
            foreach (var arg in args)
            {
                cmd.AddParameter(arg);
            }
            return cmd;
        }

        public IList<int> Transact(params NpgsqlCommand[] commands)
        {
            var result = new List<int>();
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                using (var tx = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var command in commands)
                        {
                            command.Transaction = tx;
                            command.Connection = connection;
                            result.Add(command.ExecuteNonQuery());
                        }
                        tx.Commit();
                    }
                    catch (NpgsqlException e)
                    {
                        tx.Rollback();
                        throw e;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
                return result;
            }
        }
    }
}