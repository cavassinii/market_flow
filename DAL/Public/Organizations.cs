using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Public
{
    public class Organizations
    {
        static internal string table_name = "organizations";

        public static async Task<List<DTO.Public.Organization>> GetAll()
        {
            var objs = new List<DTO.Public.Organization>();

            await using var conn = Connection.Get();
            await conn.OpenAsync();


            await using var cmd = new NpgsqlCommand(@$"
            SELECT 
            * 
            from 
            {table_name}", conn);


            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                objs.Add(Connection.MapDataReaderToObject<DTO.Public.Organization>(reader));
            }

            return objs;
        }

        public static async Task<DTO.Public.Organization> GetByName(string name)
        {
            var obj = new DTO.Public.Organization();

            await using var conn = Connection.Get();
            await conn.OpenAsync();


            await using var cmd = new NpgsqlCommand(@$"
            SELECT 
            *
            from
            {table_name}
            where
            name = @name
            ", conn);

            cmd.Parameters.AddWithValue("@name", name);


            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                obj = Connection.MapDataReaderToObject<DTO.Public.Organization>(reader);
            }

            return obj;
        }
    }
}
