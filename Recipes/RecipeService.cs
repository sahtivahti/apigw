using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using apigw.Util;

namespace apigw.Recipes
{
    public class RecipeService : IRecipeService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RecipeService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<Recipe>> GetRecipes()
        {
            using var client = _httpClientFactory.CreateClient("RecipeService");

            var result = await client.GetAsync("/v1/recipe");

            return await result.Content.ReadAsAsync<IEnumerable<Recipe>>();
        }

        public async Task<Recipe> GetRecipeById(int id)
        {
            using var client = _httpClientFactory.CreateClient("RecipeService");

            var result = await client.GetAsync($"/v1/recipe/{id}");

            return await result.Content.ReadAsAsync<Recipe>();
        }

        public async Task<Recipe> CreateRecipe(Recipe recipe)
        {
            using var client = _httpClientFactory.CreateClient("RecipeService");

            var result = await client.PostJsonAsync($"/v1/recipe", recipe);

            var body = await result.Content.ReadAsStringAsync();

            return await result.Content.ReadAsAsync<Recipe>();
        }

        public async Task<Recipe> UpdateRecipe(Recipe recipe)
        {
            if (recipe.Id == null)
            {
                throw new ArgumentNullException();
            }

            using var client = _httpClientFactory.CreateClient("RecipeService");

            var result = await client.PutJsonAsync($"/v1/recipe/{recipe.Id}", recipe);

            return await result.Content.ReadAsAsync<Recipe>();
        }
    }
}
