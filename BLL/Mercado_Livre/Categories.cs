using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using DTO.Mercado_Livre;
using Microsoft.Extensions.Options;

namespace BLL.Mercado_Livre
{
    public class Categories
    {
        public static async Task<List<MlCategory>> GetCategories()
        {
            try
            {
                var client = new RestClient("https://api.mercadolibre.com");

                var request = new RestRequest("/sites/MLB/categories/all", Method.Get);

                request.AddHeader("Authorization", "APP_USR-5058520669161994-072220-57dc2ddf46e211deb2705dcfaa75ef35-594839612");

                var response = await client.ExecuteAsync(request);

                if (!response.IsSuccessful)
                {
                    throw new Exception($"Erro na requisição: {response.StatusCode} - {response.Content}");
                }

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var dictionary = JsonSerializer.Deserialize<MlCategoryRootResponse>(response.Content, options);

                if(response.IsSuccessStatusCode)
                {
                    return dictionary?.Values.ToList() ?? new List<MlCategory>();
                }
                else
                {
                    Debug.WriteLine($"{response.Content}");
                    throw new Exception($"Erro na requisição: {response.StatusCode} - {response.Content}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter categorias do Mercado Livre.", ex);
            }
        }

    }
}
