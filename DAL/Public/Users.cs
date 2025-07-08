using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Public
{
    public class Users
    {
        static internal string table_name = "users";

        public static async Task<List<DTO.Public.User>> GetAll()
        {
            var objs = new List<DTO.Public.User>();

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
                objs.Add(Connection.MapDataReaderToObject<DTO.Public.User>(reader));
            }

            return objs;
        }

        public static async Task<DTO.Public.User> GetByName(string name, string password)
        {
            //Converto a senha para MD5, pois é assim que está armazenada no banco de dados
            //password = Convert.ToHexString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(password)));

            var obj = new DTO.Public.User();

            await using var conn = Connection.Get();
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(@$"
            SELECT 
            *
            from
            {table_name}
            where
            trim(lower(name)) = @name
            and password = @password", conn);

            cmd.Parameters.AddWithValue("@name", name.Trim().ToLower());
            cmd.Parameters.AddWithValue("@password", password.Trim().ToLower());


            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                obj = Connection.MapDataReaderToObject<DTO.Public.User>(reader);
            }

            return obj;
        }
    }
}
