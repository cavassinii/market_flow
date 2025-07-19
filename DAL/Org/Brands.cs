using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Org
{
    public class Brands
    {
        static internal string table_name = "brands";

        public static async Task<List<DTO.Org.Brand>> GetAll(string schema)
        {
            var objs = new List<DTO.Org.Brand>();
            await using var conn = Connection.Get();
            await conn.OpenAsync();
            // Define schema
            await using var setSchema = new Npgsql.NpgsqlCommand($"SET search_path TO {schema};", conn);
            await setSchema.ExecuteNonQueryAsync();
            await using var cmd = new Npgsql.NpgsqlCommand(@$"
                SELECT * FROM {table_name};
                ", conn);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                objs.Add(Connection.MapDataReaderToObject<DTO.Org.Brand>(reader));
            }
            return objs;
        }

        public static async Task<DTO.Org.Brand> GetById(string schema, int id)
        {
            var obj = new DTO.Org.Brand();
            await using var conn = Connection.Get();
            await conn.OpenAsync();
            // Define schema
            await using var setSchema = new Npgsql.NpgsqlCommand($"SET search_path TO {schema};", conn);
            await setSchema.ExecuteNonQueryAsync();
            await using var cmd = new Npgsql.NpgsqlCommand(@$"
                SELECT 
                * 
                FROM {table_name}
                WHERE
                id = @id;
                ", conn);
            cmd.Parameters.AddWithValue("@id", id);

            await using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                obj = Connection.MapDataReaderToObject<DTO.Org.Brand>(reader);
            }
            return obj;
        }

        public static async Task<int> Save(string schema, DTO.Org.Brand obj)
        {
            await using var conn = Connection.Get();
            await conn.OpenAsync();

            await using var setSchema = new NpgsqlCommand($"SET search_path TO {schema};", conn);
            await setSchema.ExecuteNonQueryAsync();

            if (obj.Id == 0)
            {
                // INSERT
                await using var cmd = new NpgsqlCommand(@$"
            INSERT INTO {table_name}
            (
                name,
                created_at,
                updated_at
            )
            VALUES
            (
                @name,
                now(),
                now()
            )
            RETURNING id;
        ", conn);

                cmd.Parameters.AddWithValue("@name", obj.Name);

                return (int)(await cmd.ExecuteScalarAsync())!;
            }
            else
            {
                // UPDATE
                await using var cmd = new NpgsqlCommand(@$"
            UPDATE {table_name}
            SET
                name = @name,
                updated_at = now()
            WHERE id = @id;
        ", conn);

                cmd.Parameters.AddWithValue("@name", obj.Name);
                cmd.Parameters.AddWithValue("@id", obj.Id);

                await cmd.ExecuteNonQueryAsync();
                return obj.Id;
            }
        }


        public static async Task<bool> DeleteById(string schema, int id)
        {
            await using var conn = Connection.Get();
            await conn.OpenAsync();
            // Define schema
            await using var setSchema = new Npgsql.NpgsqlCommand($"SET search_path TO {schema};", conn);
            await setSchema.ExecuteNonQueryAsync();

            await using var cmd = new Npgsql.NpgsqlCommand(@$"
                DELETE FROM {table_name}
                WHERE
                id = @id;
                ", conn);
            cmd.Parameters.AddWithValue("@id", id);

            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;

        }
    }
}
