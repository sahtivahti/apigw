using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using apigw.Recipes;
using apigw.Util;

namespace apigw.BeerCalculator
{
    public class BeerCalculator : IBeerCalculator
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BeerCalculator(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<CalculationResponse> GetForRecipe(Recipe recipe)
        {
            var request = new CalculationRequest
            {
                BatchSize = recipe.BatchSize,
                BoilSize = 1.2 * recipe.BatchSize,
                Hops = recipe.Hops.Select(hop => new Hop
                {
                    Weight = hop.Quantity / 1000,
                    Aa = 5.0,
                    Time = hop.Time
                }),
                Fermentables = recipe.Fermentables.Select(fermentable => new Fermentable
                {
                    Weight = fermentable.Quantity,
                    Color = fermentable.Color
                })
            };

            return await Calculate(request);
        }

        private async Task<CalculationResponse> Calculate(CalculationRequest request)
        {
            using var client = _httpClientFactory.CreateClient("BeerCalculator");

            var response = await client.PostJsonAsync("/analyze", request);

            return await response.Content.ReadAsAsync<CalculationResponse>();
        }
    }
}
