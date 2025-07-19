using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Org
{
    public class Products
    {
        static internal string table_name = "products";

        public static async Task<List<DTO.Org.Product>> GetAll(string schema)
        {
            var objs = new List<DTO.Org.Product>();
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
                objs.Add(Connection.MapDataReaderToObject<DTO.Org.Product>(reader));
            }
            return objs;
        }

        public static async Task<DTO.Org.Product> GetById(string schema, int id)
        {
            var obj = new DTO.Org.Product();
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
                obj = Connection.MapDataReaderToObject<DTO.Org.Product>(reader);
            }
            return obj;
        }

        public static async Task<int> Save(string schema, DTO.Org.Product obj)
        {
            await using var conn = Connection.Get();
            await conn.OpenAsync();

            await using var setSchema = new NpgsqlCommand($"SET search_path TO {schema};", conn);
            await setSchema.ExecuteNonQueryAsync();

            if (obj.Id == 0)
            {
                // INSERT
                await using var cmd = new NpgsqlCommand(@$"
                INSERT 
                INTO 
                {table_name}
                (
                    Updated_at,
                    Created_at,
                    Is_active,
                    Unit,
                    Height,
                    Width,
                    Weight_net,
                    Weight_gross,
                    Brand_id,
                    Category_id,                    
                    Size,
                    Color,
                    Cest,
                    Ncm,
                    Url_image5,
                    Url_image4,
                    Url_image3,
                    Url_image2,
                    Url_image1,
                    Reference,
                    Description,
                    Title,
                    Sku
                )
                VALUES
                (
                    @Updated_at,
                    @Created_at,
                    @Is_active,
                    @Unit,
                    @Height,
                    @Width,
                    @Weight_net,
                    @Weight_gross,
                    @Brand_id,
                    @Category_id,
                    @Size,
                    @Color,
                    @Cest,
                    @Ncm,
                    @Url_image5,
                    @Url_image4,
                    @Url_image3,
                    @Url_image2,
                    @Url_image1,
                    @Reference,
                    @Description,
                    @Title,
                    @Sku                  
                )
                RETURNING id;
            ", conn);

                cmd.Parameters.AddWithValue("@Updated_at", obj.Updated_at);
                cmd.Parameters.AddWithValue("@Created_at", obj.Created_at);
                cmd.Parameters.AddWithValue("@Is_active", obj.Is_active );
                cmd.Parameters.AddWithValue("@Unit", obj.Unit );
                cmd.Parameters.AddWithValue("@Height", obj.Height);
                cmd.Parameters.AddWithValue("@Width", obj.Width);
                cmd.Parameters.AddWithValue("@Weight_net", obj.Weight_net);
                cmd.Parameters.AddWithValue("@Weight_gross", obj.Weight_gross);
                cmd.Parameters.AddWithValue("@Brand_id", obj.Brand_id);
                cmd.Parameters.AddWithValue("@Category_id", obj.Category_id);
                cmd.Parameters.AddWithValue("@Size", obj.Size);
                cmd.Parameters.AddWithValue("@Color", obj.Color);
                cmd.Parameters.AddWithValue("@Cest", obj.Cest);
                cmd.Parameters.AddWithValue("@Ncm", obj.Ncm);
                cmd.Parameters.AddWithValue("@Url_image5", obj.Url_image5);
                cmd.Parameters.AddWithValue("@Url_image4", obj.Url_image4);
                cmd.Parameters.AddWithValue("@Url_image3", obj.Url_image3);
                cmd.Parameters.AddWithValue("@Url_image2", obj.Url_image2);
                cmd.Parameters.AddWithValue("@Url_image1", obj.Url_image1);
                cmd.Parameters.AddWithValue("@Reference", obj.Reference);
                cmd.Parameters.AddWithValue("@Description", obj.Description);
                cmd.Parameters.AddWithValue("@Title", obj.Title);
                cmd.Parameters.AddWithValue("@Sku", obj.Sku);

                return (int)(await cmd.ExecuteScalarAsync())!;
            }
            else
            {
                // UPDATE
                await using var cmd = new NpgsqlCommand(@$"
                UPDATE {table_name}
                SET
                    Updated_at = @Updated_at,
                    Created_at = @Created_at,
                    Is_active = @Is_active,
                    Unit = @Unit,
                    Height = @Height,
                    Width = @Width,
                    Weight_net = @Weight_net,
                    Weight_gross = @Weight_gross,
                    Brand_id = @Brand_id,
                    Category_id = @Category_id,
                    Size = @Size,
                    Color = @Color,
                    Cest = @Cest,
                    Ncm = @Ncm,
                    Url_image5 = @Url_image5,
                    Url_image4 = @Url_image4,
                    Url_image3 = @Url_image3,
                    Url_image2 = @Url_image2,
                    Url_image1 = @Url_image1,
                    Reference = @Reference,
                    Description = @Description,
                    Title = @Title,
                    Sku = @Sku
                WHERE id = @id;
            ", conn);

                cmd.Parameters.AddWithValue("@Updated_at", obj.Updated_at);
                cmd.Parameters.AddWithValue("@Created_at", obj.Created_at);
                cmd.Parameters.AddWithValue("@Is_active", obj.Is_active);
                cmd.Parameters.AddWithValue("@Unit", obj.Unit);
                cmd.Parameters.AddWithValue("@Height", obj.Height);
                cmd.Parameters.AddWithValue("@Width", obj.Width);
                cmd.Parameters.AddWithValue("@Weight_net", obj.Weight_net);
                cmd.Parameters.AddWithValue("@Weight_gross", obj.Weight_gross);
                cmd.Parameters.AddWithValue("@Brand_id", obj.Brand_id);
                cmd.Parameters.AddWithValue("@Category_id", obj.Category_id);
                cmd.Parameters.AddWithValue("@Size", obj.Size);
                cmd.Parameters.AddWithValue("@Color", obj.Color);
                cmd.Parameters.AddWithValue("@Cest", obj.Cest);
                cmd.Parameters.AddWithValue("@Ncm", obj.Ncm);
                cmd.Parameters.AddWithValue("@Url_image5", obj.Url_image5);
                cmd.Parameters.AddWithValue("@Url_image4", obj.Url_image4);
                cmd.Parameters.AddWithValue("@Url_image3", obj.Url_image3);
                cmd.Parameters.AddWithValue("@Url_image2", obj.Url_image2);
                cmd.Parameters.AddWithValue("@Url_image1", obj.Url_image1);
                cmd.Parameters.AddWithValue("@Reference", obj.Reference);
                cmd.Parameters.AddWithValue("@Description", obj.Description);
                cmd.Parameters.AddWithValue("@Title", obj.Title);
                cmd.Parameters.AddWithValue("@Sku", obj.Sku);
                cmd.Parameters.AddWithValue("@Id", obj.Id);

                await cmd.ExecuteNonQueryAsync();
                return obj.Id;
            }
        }

        public static async Task<bool> DeleteById(string schema, int id)
        {
            var obj = new DTO.Org.Product();
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
