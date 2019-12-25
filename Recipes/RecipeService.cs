using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace apigw.Recipes
{
    public class RecipeService : IRecipeService
    {
        private readonly HttpClient _client;

        public RecipeService(string baseUri)
        {
            _client = new HttpClient { BaseAddress = new Uri(baseUri) };
        }

        public async Task<IEnumerable<Recipe>> GetRecipes()
        {
            var result = await _client.GetAsync("/v1/recipe");

            var body = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IEnumerable<Recipe>>(body);
        }

        public async Task<Recipe> GetRecipeById(int id)
        {
            var result = await _client.GetAsync($"/v1/recipe/{id}");

            var body = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Recipe>(body);
        }

        public async Task<Recipe> UpdateRecipe(Recipe recipe)
        {
            if (recipe.Id == null) {
                throw new ArgumentNullException();
            }

            var result = await _client.PutAsync<Recipe>($"/v1/recipe/{recipe.Id}", recipe, GetFormatter());
            var body = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Recipe>(body);
        }

        private static JsonMediaTypeFormatter GetFormatter()
        {
            var formatter = new JsonMediaTypeFormatter();

            formatter.SerializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            return formatter;
        }
    }
}
