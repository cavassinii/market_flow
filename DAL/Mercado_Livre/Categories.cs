using DTO;
using DTO.Mercado_Livre;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DAL.Mercado_Livre
{
    public class Categories
    {
        private static string schema = "mercado_livre";

        // Insert ou update na tabela principal
        public static async Task InsertOrUpdateCategory(NpgsqlConnection conn, MlCategory category)
        {
            var sql = @"
            INSERT INTO mercado_livre.ml_categories 
                (ml_id, name, picture, permalink, total_items, attribute_types, meta_categ_id, attributable, date_created)
            VALUES
                (@ml_id, @name, @picture, @permalink, @total_items, @attribute_types, @meta_categ_id, @attributable, @date_created)
            ON CONFLICT (ml_id) DO UPDATE SET
                name = EXCLUDED.name,
                picture = EXCLUDED.picture,
                permalink = EXCLUDED.permalink,
                total_items = EXCLUDED.total_items,
                attribute_types = EXCLUDED.attribute_types,
                meta_categ_id = EXCLUDED.meta_categ_id,
                attributable = EXCLUDED.attributable,
                date_created = EXCLUDED.date_created;
        ";

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("ml_id", category.Id);
            cmd.Parameters.AddWithValue("name", category.Name);
            cmd.Parameters.AddWithValue("picture", (object?)category.Picture ?? DBNull.Value);
            cmd.Parameters.AddWithValue("permalink", (object?)category.Permalink ?? DBNull.Value);
            cmd.Parameters.AddWithValue("total_items", category.TotalItemsInThisCategory);
            cmd.Parameters.AddWithValue("attribute_types", (object?)category.AttributeTypes ?? DBNull.Value);
            cmd.Parameters.AddWithValue("meta_categ_id", (object?)category.MetaCategId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("attributable", category.Attributable);
            cmd.Parameters.AddWithValue("date_created", (object?)category.DateCreated ?? DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }

        // Insert path_from_root (limpa antes e insere tudo de novo)
        public static async Task InsertCategoryPath(NpgsqlConnection conn, MlCategory category)
        {
            var deleteSql = "DELETE FROM mercado_livre.ml_category_path WHERE ml_category_id = @ml_id;";
            await using var deleteCmd = new NpgsqlCommand(deleteSql, conn);
            deleteCmd.Parameters.AddWithValue("ml_id", category.Id);
            await deleteCmd.ExecuteNonQueryAsync();

            var insertSql = @"
            INSERT INTO mercado_livre.ml_category_path (ml_category_id, position, ancestor_id, ancestor_name)
            VALUES (@ml_id, @pos, @ancestor_id, @ancestor_name);
        ";

            int pos = 0;
            foreach (var ancestor in category.PathFromRoot)
            {
                await using var cmd = new NpgsqlCommand(insertSql, conn);
                cmd.Parameters.AddWithValue("ml_id", category.Id);
                cmd.Parameters.AddWithValue("pos", pos++);
                cmd.Parameters.AddWithValue("ancestor_id", ancestor.Id);
                cmd.Parameters.AddWithValue("ancestor_name", ancestor.Name);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        // Insert children_categories (limpa antes e insere tudo)
        public static async Task InsertChildrenCategories(NpgsqlConnection conn, MlCategory category)
        {
            var deleteSql = "DELETE FROM mercado_livre.ml_children_categories WHERE ml_category_id = @ml_id;";
            await using var deleteCmd = new NpgsqlCommand(deleteSql, conn);
            deleteCmd.Parameters.AddWithValue("ml_id", category.Id);
            await deleteCmd.ExecuteNonQueryAsync();

            var insertSql = @"
            INSERT INTO mercado_livre.ml_children_categories (ml_category_id, child_id, child_name, total_items)
            VALUES (@ml_id, @child_id, @child_name, @total_items);
        ";

            foreach (var child in category.ChildrenCategories)
            {
                await using var cmd = new NpgsqlCommand(insertSql, conn);
                cmd.Parameters.AddWithValue("ml_id", category.Id);
                cmd.Parameters.AddWithValue("child_id", child.Id);
                cmd.Parameters.AddWithValue("child_name", child.Name);
                cmd.Parameters.AddWithValue("total_items", child.TotalItemsInThisCategory);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        // Insert settings JSON
        public static async Task InsertCategorySettings(NpgsqlConnection conn, MlCategory category)
        {
            var settingsJson = JsonSerializer.Serialize(category.Settings);

            var sql = @"
            INSERT INTO mercado_livre.ml_category_settings (ml_category_id, settings_json)
            VALUES (@ml_id, @settings_json)
            ON CONFLICT (ml_category_id) DO UPDATE SET
                settings_json = EXCLUDED.settings_json;
        ";

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("ml_id", category.Id);
            cmd.Parameters.AddWithValue("settings_json", NpgsqlTypes.NpgsqlDbType.Jsonb, settingsJson);

            await cmd.ExecuteNonQueryAsync();
        }

        // Insert channels settings (limpa antes e insere todos)
        public static async Task InsertCategoryChannelSettings(NpgsqlConnection conn, MlCategory category)
        {
            var deleteSql = "DELETE FROM mercado_livre.ml_category_channel_settings WHERE ml_category_id = @ml_id;";
            await using var deleteCmd = new NpgsqlCommand(deleteSql, conn);
            deleteCmd.Parameters.AddWithValue("ml_id", category.Id);
            await deleteCmd.ExecuteNonQueryAsync();

            var insertSql = @"
            INSERT INTO mercado_livre.ml_category_channel_settings (ml_category_id, channel, settings_json)
            VALUES (@ml_id, @channel, @settings_json);
        ";

            foreach (var channel in category.ChannelsSettings)
            {
                var settingsJson = JsonSerializer.Serialize(channel.Settings);
                await using var cmd = new NpgsqlCommand(insertSql, conn);
                cmd.Parameters.AddWithValue("ml_id", category.Id);
                cmd.Parameters.AddWithValue("channel", channel.Channel);
                cmd.Parameters.AddWithValue("settings_json", NpgsqlTypes.NpgsqlDbType.Jsonb, settingsJson);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        // Função geral que insere uma lista inteira
        public static async Task InsertCategoriesBatch(NpgsqlConnection conn, List<MlCategory> categories)
        {
            // Abre a conexão se ainda não estiver aberta
            if (conn.State != System.Data.ConnectionState.Open)
                await conn.OpenAsync();

            // Começa a transação
            await using var transaction = await conn.BeginTransactionAsync();

            try
            {
                foreach (var category in categories)
                {
                    await InsertOrUpdateCategory(conn, category);
                    await InsertCategoryPath(conn, category);
                    await InsertChildrenCategories(conn, category);
                    await InsertCategorySettings(conn, category);
                    await InsertCategoryChannelSettings(conn, category);
                }

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public static async Task<List<CategoryTreeNode>> GetFullCategoryTreeAsync()
        {
            await using var conn = Connection.Get();
            await conn.OpenAsync();

            const string sql = @"
    WITH RECURSIVE category_tree AS (
        SELECT
            c.ml_id,
            c.name,
            NULL::text AS parent_ml_id,
            0 AS level
        FROM mercado_livre.ml_categories c
        WHERE NOT EXISTS (
            SELECT 1 FROM mercado_livre.ml_children_categories ch
            WHERE ch.child_id = c.ml_id
        )
        UNION ALL
        SELECT
            c.ml_id,
            c.name,
            ch.ml_category_id AS parent_ml_id,
            ct.level + 1 AS level
        FROM mercado_livre.ml_children_categories ch
        JOIN mercado_livre.ml_categories c ON c.ml_id = ch.child_id
        JOIN category_tree ct ON ct.ml_id = ch.ml_category_id
    )
    SELECT ml_id, name, parent_ml_id FROM category_tree ORDER BY level, parent_ml_id NULLS FIRST, ml_id;
    ";

            var nodesDict = new Dictionary<string, CategoryTreeNode>();

            await using var cmd = new NpgsqlCommand(sql, conn);
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var mlId = reader.GetString(0);
                var name = reader.GetString(1);
                var parentMlId = reader.IsDBNull(2) ? null : reader.GetString(2);

                var node = new CategoryTreeNode
                {
                    MlId = mlId,
                    Name = name,
                    ParentMlId = parentMlId,
                    Children = new List<CategoryTreeNode>()
                };

                nodesDict[mlId] = node;
            }

            // Montar a árvore
            var roots = new List<CategoryTreeNode>();

            foreach (var node in nodesDict.Values)
            {
                if (node.ParentMlId == null)
                {
                    roots.Add(node);
                }
                else if (nodesDict.TryGetValue(node.ParentMlId, out var parent))
                {
                    parent.Children.Add(node);
                }
            }

            return roots;
        }

    }
}
