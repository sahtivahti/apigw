using System.Linq;
using System.Collections.Generic;
using apigw.Pact;
using apigw.Recipes;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using Xunit;

namespace apigw.Test
{
    public class RecipeServiceConsumerTest : IClassFixture<RecipeServicePact>
    {
        private IMockProviderService _mockProviderService;
        private string _mockProviderServiceBaseUri;

        public RecipeServiceConsumerTest(RecipeServicePact data)
        {
            _mockProviderService = data.MockProviderService;
            _mockProviderService.ClearInteractions();
            _mockProviderServiceBaseUri = data.MockProviderServiceBaseUri;
        }

        [Fact]
        public async void GetRecipes_WillReturn200()
        {
            var response = new List<object>();
            response.Add(new { name = "My another recipe", author = "panomestari@sahtivahti.fi" });

            _mockProviderService
                .Given("There is some recipes")
                .UponReceiving("GET request to retrieve recipes")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/v1/recipe"
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Body = response,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json" }
                    }
                });

            var consumer = new RecipeService(_mockProviderServiceBaseUri);

            var result = await consumer.GetRecipes();

            Assert.Equal(result.Count(), response.Count());
            _mockProviderService.VerifyInteractions();
        }

        [Fact]
        public async void GetRecipeById_WillReturn200()
        {
            _mockProviderService
                .Given("There is a recipe with id 1")
                .UponReceiving("GET request to retrieve recipe")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/v1/recipe/1"
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Body = new {
                        id = 1,
                        name = "My another recipe",
                        author = "panomestari@sahtivahti.fi"
                    },
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json" }
                    }
                });

            var consumer = new RecipeService(_mockProviderServiceBaseUri);
            var result = await consumer.GetRecipeById(1);

            Assert.Equal("My another recipe", result.Name);
            _mockProviderService.VerifyInteractions();
        }

        [Fact]
        public async void UpdateRecipe_WillReturn200()
        {
            var body = new
            {
                id = 1,
                name = "My updated recipe",
                author = "panomestari@sahtivahti.fi"
            };

            var recipe = new Recipe
            {
                Id = 1,
                Name = "My updated recipe",
                Author = "panomestari@sahtivahti.fi"
            };

            _mockProviderService
                .Given("There is a recipe with id 1")
                .UponReceiving("PUT to update recipe")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Put,
                    Path = "/v1/recipe/1",
                    Body = body,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json; charset=utf-8" },
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json" }
                    },
                    Body = body
                });

            var consumer = new RecipeService(_mockProviderServiceBaseUri);
            var result = await consumer.UpdateRecipe(recipe);

            Assert.Equal(recipe.Name, result.Name);
            _mockProviderService.VerifyInteractions();
        }
    }
}
