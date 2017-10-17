using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Npgsql;

namespace Chinook.Model.Data
{
    public interface IRunner
    {
        IEnumerable<object> ExecuteDynamic(string command, params object[] args);
        dynamic ExecuteToSingleDynamic(string command, params object[] args);
        T ExecuteToSingle<T>(string command, params object[] args) where T : new();
        IEnumerable<T> Execute<T>(string sqlCommand, params object[] args) where T : new();
        string ConnectionString { get; set; }
        NpgsqlDataReader OpenReader(string sql, params object[] args);
        Task<NpgsqlDataReader> OpenReaderAsync(string sql, params object[] args);
        NpgsqlCommand BuildCommand(string sql,NpgsqlConnection connection, params object[] args);
        NpgsqlCommand BuildCommand(string sql,params object[] args);
        IList<int> Transact(params NpgsqlCommand[] commands);
    }
}