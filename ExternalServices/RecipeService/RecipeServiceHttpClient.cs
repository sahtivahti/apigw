using System.Net.Http;
using System.Threading.Tasks;
using apigw.ExternalServices.RecipeService.Model;
using apigw.Util;
using System.Net;
using System.Collections.Generic;

namespace apigw.ExternalServices.RecipeService
{
    public class RecipeServiceHttpClient : IRecipeServiceClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RecipeServiceHttpClient(
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<FermentableDetailsResponse> AddFermentableById(int recipeId, CreateFermentableRequest request)
        {
            using var client = CreateClient();

            var result = await client.PostJsonAsync($"/v1/recipe/{recipeId}/fermentable", request);

            return await result.Content.ReadAsAsync<FermentableDetailsResponse>();
        }

        public async Task<HopDetailsResponse> AddHopById(int recipeId, CreateHopRequest request)
        {
            using var client = CreateClient();

            var result = await client.PostJsonAsync($"/v1/recipe/{recipeId}/hop", request);

            return await result.Content.ReadAsAsync<HopDetailsResponse>();
        }

        public async Task<RecipeDetailsResponse> Create(CreateRecipeRequest request)
        {
            using var client = CreateClient();

            var result = await client.PostJsonAsync($"/v1/recipe", request);

            return await result.Content.ReadAsAsync<RecipeDetailsResponse>();
        }

        public async Task DeleteById(int recipeId)
        {
            using var client = CreateClient();

            var result = await client.DeleteAsync($"/v1/recipe/{recipeId}");

            if (result.StatusCode == HttpStatusCode.NotFound)
            {
                throw new RecipeNotFoundException();
            }
        }

        public async Task<RecipeDetailsResponse> GetById(int recipeId)
        {
            using var client = CreateClient();

            var result = await client.GetAsync("/v1/recipe/" + recipeId);

            if (result.StatusCode == HttpStatusCode.NotFound)
            {
                throw new RecipeNotFoundException();
            }

            return await result.Content.ReadAsAsync<RecipeDetailsResponse>();
        }

        public async Task DeleteFermentableById(int recipeId, int fermentableId)
        {
            using var client = CreateClient();

            var result = await client.DeleteAsync($"/v1/recipe/{recipeId}/fermentable/{fermentableId}");

            if (result.StatusCode == HttpStatusCode.NotFound)
            {
                throw new RecipeNotFoundException();
            }
        }

        public async Task DeleteHopById(int recipeId, int hopId)
        {
            using var client = CreateClient();

            var result = await client.DeleteAsync($"/v1/recipe/{recipeId}/hop/{hopId}");

            if (result.StatusCode == HttpStatusCode.NotFound)
            {
                throw new RecipeNotFoundException();
            }
        }

        public async Task<RecipeDetailsResponse> UpdateById(int recipeId, UpdateRecipeRequest request)
        {
            using var client = CreateClient();

            var result = await client.PutJsonAsync($"/v1/recipe/{recipeId}", request);

            if (result.StatusCode == HttpStatusCode.NotFound)
            {
                throw new RecipeNotFoundException();
            }

            return await result.Content.ReadAsAsync<RecipeDetailsResponse>();
        }

        public async Task<IEnumerable<RecipeListItem>> GetRecipes()
        {
            using var client = CreateClient();

            var result = await client.GetAsync("/v1/recipe");

            return await result.Content.ReadAsAsync<IEnumerable<RecipeListItem>>();
        }

        private HttpClient CreateClient()
        {
            return _httpClientFactory.CreateClient("RecipeService");
        }
    }
}
