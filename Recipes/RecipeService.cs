using System.Net;
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

            if (result.StatusCode == HttpStatusCode.NotFound)
            {
                throw new RecipeNotFoundException();
            }

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

            if (result.StatusCode == HttpStatusCode.NotFound)
            {
                throw new RecipeNotFoundException();
            }

            return await result.Content.ReadAsAsync<Recipe>();
        }

        public async Task RemoveRecipeById(int id)
        {
            using var client = _httpClientFactory.CreateClient("RecipeService");

            var result = await client.DeleteAsync($"/v1/recipe/{id}");

            if (result.StatusCode == HttpStatusCode.NotFound)
            {
                throw new RecipeNotFoundException();
            }
        }

        public async Task<Hop> AddHopToRecipe(Hop hop, int recipeId)
        {
            using var client = _httpClientFactory.CreateClient("RecipeService");

            var result = await client.PostJsonAsync($"/v1/recipe/{recipeId}/hop", hop);

            if (result.StatusCode == HttpStatusCode.NotFound)
            {
                throw new RecipeNotFoundException();
            }

            return await result.Content.ReadAsAsync<Hop>();
        }

        public async Task<Hop> RemoveHopFromRecipe(int hopId, int recipeId)
        {
            using var client = _httpClientFactory.CreateClient("RecipeService");

            var result = await client.DeleteAsync($"/v1/recipe/{recipeId}/hop/{hopId}");

            if (result.StatusCode == HttpStatusCode.NotFound)
            {
                throw new RecipeNotFoundException();
            }

            return await result.Content.ReadAsAsync<Hop>();
        }

        public async Task<Fermentable> AddFermentableToRecipe(Fermentable fermentable, int recipeId)
        {
            using var client = _httpClientFactory.CreateClient("RecipeService");

            var result = await client.PostJsonAsync($"/v1/recipe/{recipeId}/fermentable", fermentable);

            if (result.StatusCode == HttpStatusCode.NotFound)
            {
                throw new RecipeNotFoundException();
            }

            return await result.Content.ReadAsAsync<Fermentable>();
        }

        public async Task<Fermentable> RemoveFermentableFromRecipe(int fermentableId, int recipeId)
        {
            using var client = _httpClientFactory.CreateClient("RecipeService");

            var result = await client.DeleteAsync($"/v1/recipe/{recipeId}/fermentable/{fermentableId}");

            if (result.StatusCode == HttpStatusCode.NotFound)
            {
                throw new RecipeNotFoundException();
            }

            return await result.Content.ReadAsAsync<Fermentable>();
        }
    }
}
