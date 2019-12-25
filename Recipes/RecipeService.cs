using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using apigw.Util;

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

            return await result.Content.ReadAsAsync<IEnumerable<Recipe>>();
        }

        public async Task<Recipe> GetRecipeById(int id)
        {
            var result = await _client.GetAsync($"/v1/recipe/{id}");

            return await result.Content.ReadAsAsync<Recipe>();
        }

        public async Task<Recipe> CreateRecipe(Recipe recipe)
        {
            var result = await _client.PostJsonAsync($"/v1/recipe", recipe);
            var body = await result.Content.ReadAsStringAsync();

            return await result.Content.ReadAsAsync<Recipe>();
        }

        public async Task<Recipe> UpdateRecipe(Recipe recipe)
        {
            if (recipe.Id == null) {
                throw new ArgumentNullException();
            }

            var result = await _client.PutJsonAsync($"/v1/recipe/{recipe.Id}", recipe);

            return await result.Content.ReadAsAsync<Recipe>();
        }
    }
}
