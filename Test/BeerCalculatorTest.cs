using System;
using System.Collections.Generic;
using apigw.Pact;
using PactNet.Mocks.MockHttpService;
using Xunit;
using Moq;
using System.Net.Http;
using PactNet.Mocks.MockHttpService.Models;
using apigw.Recipes;

namespace apigw.Test
{
    public class BeerCalculatorConsumerTest : IClassFixture<BeerCalculatorPact>
    {
        private IMockProviderService _mockProviderService;
        private string _mockProviderServiceBaseUri;

        public BeerCalculatorConsumerTest(BeerCalculatorPact data)
        {
            _mockProviderService = data.MockProviderService;
            _mockProviderService.ClearInteractions();
            _mockProviderServiceBaseUri = data.MockProviderServiceBaseUri;
        }

        [Fact]
        public async void AnalyzeRecipe_WillReturn200()
        {
            var recipe = new Recipe
            {
                BatchSize = 10,
                Hops = new List<Hop>
                {
                    new Hop
                    {
                        Quantity = 20,
                        Time = 60
                    }
                },
                Fermentables = new List<Fermentable>
                {
                    new Fermentable
                    {
                        Quantity = 5,
                        Color = 7
                    }
                }
            };

            _mockProviderService
                .Given("The service is up and running")
                .UponReceiving("POST request to analyze recipe")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Post,
                    Path = "/analyze",
                    Body = new
                    {
                        batchSize = 10,
                        boilSize = 12,
                        hops = new[]
                        {
                            new
                            {
                                aa = 5.0,
                                weight = 0.02,
                                time = 60
                            }
                        },
                        fermentables = new[]
                        {
                            new
                            {
                                weight = 5,
                                color = 7
                            }
                        }
                    },
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json; charset=utf-8" }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Body = new
                    {
                        og = 1.1,
                        fg = 1.02,
                        abv = 10,
                        ibu = 60,
                        colorSrm = 12,
                        colorEbc = 16,
                        colorName = "amber"
                    },
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json" }
                    }
                });

            var consumer = CreateConsumer();

            var result = await consumer.GetForRecipe(recipe);

            Assert.Equal(1.1, result.Og);
            Assert.Equal(1.02, result.Fg);
            Assert.Equal(10, result.Abv);
            Assert.Equal(60, result.Ibu);
            Assert.Equal(12, result.ColorSrm);
            Assert.Equal(16, result.ColorEbc);
            Assert.Equal("amber", result.ColorName);

            _mockProviderService.VerifyInteractions();
        }

        private BeerCalculator.BeerCalculator CreateConsumer()
        {
            var mockFactory = new Mock<IHttpClientFactory>();
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>()))
                .Returns(new HttpClient
                {
                    BaseAddress = new Uri(_mockProviderServiceBaseUri)
                });

            return new BeerCalculator.BeerCalculator(mockFactory.Object);
        }
    }
}
