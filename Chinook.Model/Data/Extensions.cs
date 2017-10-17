using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using Npgsql;

namespace Chinook.Model.Data
{
    public static class Extensions
    {
        public static IEnumerable<dynamic> DynamicList(this IDataReader reader)
        {
            while (reader.Read())
            {
                yield return reader.ToExpando();
            }
        }
        public static dynamic ToExpando(this IDataReader reader)
        {
            dynamic e = new ExpandoObject();
            var d  = e as IDictionary<string, object>;
            for (int i = 0; i < reader.FieldCount; i++)
            {
                TextInfo textInfo = new CultureInfo("en-US",false).TextInfo;
                var replacedName = reader.GetName(i).Replace("_", " ").ToLower();
                var name = textInfo.ToTitleCase(replacedName).Replace(" ",string.Empty);
                d.Add(name,DBNull.Value.Equals(reader.GetValue(i)) ? null : reader.GetValue(i));
            }
            return e;
        }

        public static List<T> ToList<T>(this IDataReader rdr) where T : new()
        {
            var result = new List<T>();
            while (rdr.Read())
            {
                result.Add(rdr.ToSingle<T>());
            }
            return result;
        }

        public static string GetColumnName(this IDataReader reader)
        {
            return reader.GetValue(0).ToString();
        }
        public static T ToSingle<T>(this IDataReader reader) where T : new()
        {
            var item = new T();
            var properties = item.GetType().GetProperties();
            foreach (var property in properties)
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (property.Name.Equals(reader.GetName(i), StringComparison.InvariantCultureIgnoreCase))
                    {
                        property.SetValue(item,DBNull.Value.Equals(reader.GetValue(i)) ? null : reader.GetValue(i));
                    }
                }
            }
            return item;
        }

        public static void AddParameter(this NpgsqlCommand command, object arg)
        {
            var paramName = string.Format("@{0}", command.Parameters.Count);
            var parameter = new NpgsqlParameter
            {
                ParameterName = paramName
            };
            if (arg == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                if (arg is Guid)
                {
                    parameter.DbType = DbType.Guid;
                    parameter.Value = arg.ToString();
                    parameter.Size = 4000;
                }
                else
                {
                    parameter.Value = arg;
                }
                if (arg is string)
                {
                    parameter.Size = ((string) arg).Length > 4000 ? -1 : 4000;
                }
            }
            command.Parameters.Add(parameter);
        }
    }
}