using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;

namespace Chinook.Model.Data
{
    public class Query<T> : IQuery<T> where T : new()
    {
        public IRunner Runner { get; set; }
        private string TableName { get; set; }
        private string IdColumn { get; set; }
        private List<string> PropertiesNames { get; set; }
        public Query(IRunner runner)
        {
            Runner = runner;
            TableName = typeof(T).Name.ToLower();
            IdColumn = TableName + "_id";
            GetAllColumnNames();
            PropertiesNames = new List<string>();
            PropertiesNames.AddRange(typeof(T)
                .GetProperties()
                .Where(p => p.PropertyType != typeof(IList) || p.PropertyType != typeof(IEnumerable))
                .Select(p => p.Name).ToList());
        }

        private void GetAllColumnNames()
        {
            Columns = new List<string>();
            var cmdSql = string.Format("SELECT column_name FROM information_schema.columns WHERE table_schema ='{0}' AND table_name='{1}'","public",TableName);
            using (var reader = Runner.OpenReader(cmdSql,null))
            {
                while (reader.Read())
                {
                    Columns.Add(reader.GetColumnName());
                }
            } 
        }

        private IList<string> Columns { get; set; }

        public IEnumerable<T> GetAll()
        {
            // get table name
            // get properties
            // get columns Name
            StringBuilder select = new StringBuilder();
            for(var i=0; i< PropertiesNames.Count;i++)
            {
                select.Append(Columns[i]);
                select.Append(" as ");
                select.Append(PropertiesNames[i]);
                if(i != PropertiesNames.Count - 1)
                    select.Append(" ,");
            }
            var sql = string.Format("select {0} from {1}",select.ToString(),TableName);
            return Runner.Execute<T>(sql, null);
        }

        public T GetById(int id)
        {
            StringBuilder select = new StringBuilder();
            for (int i = 0; i < PropertiesNames.Count; i++)
            {
                select.Append(Columns[i]);
                select.Append(" as ");
                select.Append(PropertiesNames[i]);
                if(i != PropertiesNames.Count - 1)
                    select.Append(" ,");
            }
            var sql = string.Format("select {0} from {1} where {2} =@0",select.ToString(),TableName,IdColumn);
            return Runner.ExecuteToSingle<T>(sql, id);
        }

        public T Refrence<T1>(int id) where T1 : new()
        {
            var result = default(T);
            var primarySql = @"select {0} from {1} where {2}=@0";
            var includeSql = @"select {0} from {1} where {2}=@0";
            StringBuilder select = new StringBuilder();
            for (int i = 0; i < PropertiesNames.Count; i++)
            {
                select.Append(Columns[i]);
                select.Append(" as ");
                select.Append(PropertiesNames[i]);
                if(i != PropertiesNames.Count - 1)
                    select.Append(" ,");
            }
            return result;
        }
    }
}