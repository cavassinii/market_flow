using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Connection
    {
        public static NpgsqlConnection Get()
        {
            return new NpgsqlConnection(@"
            Server=localhost;
            Port=5432;
            User Id=postgres;
            Password=123;
            Database=market_flow;
            CommandTimeout=5000
            ");
        }

        public static T MapDataReaderToObject<T>(IDataReader dr) where T : new()
        {
            var obj = new T();
            var props = typeof(T).GetProperties();

            for (int i = 0; i < dr.FieldCount; i++)
            {
                var columnName = dr.GetName(i);
                var prop = props.FirstOrDefault(p => string.Equals(p.Name, columnName, StringComparison.OrdinalIgnoreCase));

                if (prop != null && dr[i] != DBNull.Value)
                {
                    var propType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                    // Trata caso específico de List<string> vindo de array do PostgreSQL
                    if (propType == typeof(List<string>) && dr[i] is string[] stringArray)
                    {
                        prop.SetValue(obj, stringArray.ToList());
                    }
                    else
                    {
                        var value = Convert.ChangeType(dr[i], propType);
                        prop.SetValue(obj, value);
                    }
                }
            }

            return obj;
        }


    }
}
