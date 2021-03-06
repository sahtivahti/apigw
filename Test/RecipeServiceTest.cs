using System;
using System.Linq;
using System.Collections.Generic;
using apigw.Pact;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using Xunit;
using Moq;
using System.Net.Http;
using apigw.ExternalServices.RecipeService;
using apigw.ExternalServices.RecipeService.Model;

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

            response.Add(
                new
                {
                    name = "My another recipe",
                    author = "panomestari@sahtivahti.fi",
                    userId = "auth0|foobar",
                    style = "IPA"
                }
            );

            _mockProviderService
                .Given("There is some recipes")
                .UponReceiving("GET request to retrieve recipes")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/v1/recipe",
                    Query = "userId=auth0userid"
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

            var consumer = CreateConsumer();

            var result = await consumer.SearchRecipes(new RecipeSearchFilters
            {
                UserId = "auth0userid"
            });

            Assert.Equal(result.Count(), response.Count());
            _mockProviderService.VerifyInteractions();
        }

        [Fact]
        public async void GetRecipeByIdThatNotExist_WillReturn404()
        {
            _mockProviderService
                .Given("There is no recipe with id 666")
                .UponReceiving("GET request to retrieve recipe")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/v1/recipe/666"
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 404
                });

            var consumer = CreateConsumer();

            await Assert.ThrowsAsync<RecipeNotFoundException>(
                async () => await consumer.GetById(666)
            );

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
                        author = "panomestari@sahtivahti.fi",
                        userId = "auth0|foobar",
                        style = "IPA",
                        batchSize = 20.5
                    },
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json" }
                    }
                });

            var consumer = CreateConsumer();
            var result = await consumer.GetById(1);

            Assert.Equal("My another recipe", result.Name);
            _mockProviderService.VerifyInteractions();
        }

        [Fact]
        public async void UpdateRecipe_WillReturn200()
        {
            var body = new
            {
                name = "My updated recipe",
                author = "panomestari@sahtivahti.fi",
                style = "IPA",
                batchSize = 20.5
            };

            var request = new UpdateRecipeRequest
            {
                Name = "My updated recipe",
                Author = "panomestari@sahtivahti.fi",
                Style = "IPA",
                BatchSize = 20.5
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

            var consumer = CreateConsumer();
            var result = await consumer.UpdateById(1, request);

            Assert.Equal(request.Name, result.Name);
            _mockProviderService.VerifyInteractions();
        }

        [Fact]
        public async void CreateRecipe_WillReturn201()
        {
            var recipe = new CreateRecipeRequest
            {
                Name = "My new recipe!",
                Author = "panomies@sahtivahti.fi",
                UserId = "auth0|foobar",
                BatchSize = 20.5,
                Style = "Belgian Pale Ale"
            };

            _mockProviderService
                .Given("There is no recipes for panomies")
                .UponReceiving("POST to create recipe")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Post,
                    Path = "/v1/recipe",
                    Body = new {
                        name = "My new recipe!",
                        author = "panomies@sahtivahti.fi",
                        userId = "auth0|foobar",
                        batchSize = 20.5,
                        style = "Belgian Pale Ale"
                    },
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json; charset=utf-8" },
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 201,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json" }
                    },
                    Body = new {
                        name = "My new recipe!",
                        author = "panomies@sahtivahti.fi",
                        userId = "auth0|foobar",
                        batchSize = 20.5,
                        style = "Belgian Pale Ale",
                        hops = new string[]{},
                        fermentables = new string[]{}
                    }
                });

            var consumer = CreateConsumer();
            var result = await consumer.Create(recipe);

            Assert.Equal(recipe.Name, result.Name);
            _mockProviderService.VerifyInteractions();
        }

        private RecipeServiceHttpClient CreateConsumer()
        {
            var httpClientMock = new Mock<IHttpClientFactory>();
            httpClientMock.Setup(_ => _.CreateClient(It.IsAny<string>()))
                .Returns(new HttpClient
                {
                    BaseAddress = new Uri(_mockProviderServiceBaseUri)
                });

            return new RecipeServiceHttpClient(httpClientMock.Object);
        }
    }
}
