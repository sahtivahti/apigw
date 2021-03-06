using System;
using System.Threading.Tasks;
using apigw.Cache;
using apigw.ExternalServices.BeerCalculator;
using apigw.ExternalServices.RecipeService;
using apigw.ExternalServices.RecipeService.Model;
using apigw.Recipes;
using apigw.Recipes.Model;
using Moq;
using Xunit;

namespace apigw.Test.Integration.Recipes
{
    public class RecipeServiceTest
    {
        private readonly Mock<IRecipeServiceClient> _recipeServiceClientMock;
        private readonly Mock<IBeerCalculator> _beerCalculatorMock;
        private readonly Mock<ICache<RecipeDetails>> _cacheMock;

        public RecipeServiceTest()
        {
            _recipeServiceClientMock = new Mock<IRecipeServiceClient>();
            _beerCalculatorMock = new Mock<IBeerCalculator>();
            _cacheMock = new Mock<ICache<RecipeDetails>>();
        }

        [Fact]
        public async void GetRecipeDetails_WillCombineResultsFromRequiredServices()
        {
            _recipeServiceClientMock.Setup(_ => _.GetById(It.IsAny<int>()))
                .Returns(Task.FromResult(new RecipeDetailsResponse
                {
                    Id = 1,
                    Name = "Foo",
                    BatchSize = 20,
                    Style = "IPA",
                    Author = "sahti.vahti@sahtivahti.fi",
                    UserId = "foobaruser"
                }));

            _beerCalculatorMock.Setup(_ => _.Calculate(It.IsAny<CalculationRequest>()))
                .Returns(Task.FromResult(new CalculationResponse
                {
                    Abv = 6.0,
                    Fg = 1.01,
                    Og = 1.056,
                    ColorName = "brown",
                    ColorEbc = 7.6,
                    Ibu = 50
                }));

            var recipeService = new RecipeService(
                _recipeServiceClientMock.Object,
                _beerCalculatorMock.Object,
                _cacheMock.Object
            );

            var recipe = await recipeService.GetRecipeById(1);

            Assert.Equal(1, recipe.Id);
            Assert.Equal("Foo", recipe.Name);
            Assert.Equal("IPA", recipe.Style);
            Assert.Equal(20, recipe.BatchSize);
            Assert.Equal("sahti.vahti@sahtivahti.fi", recipe.Author);
            Assert.Equal(6.0, recipe.Abv);
            Assert.Equal(1.01, recipe.FinalGravity);
            Assert.Equal(1.056, recipe.OriginalGravity);
            Assert.Equal("brown", recipe.ColorName);
            Assert.Equal(7.6, recipe.Color);
            Assert.Equal(50, recipe.Ibu);
            Assert.Equal("foobaruser", recipe.UserId);
        }

        [Fact]
        public async void GetRecipesForUser_CallsClientWithProperFilters()
        {
            var recipeService = new RecipeService(
                _recipeServiceClientMock.Object,
                _beerCalculatorMock.Object,
                _cacheMock.Object
            );

            await recipeService.GetRecipesForUser("foobaruserid");

            _recipeServiceClientMock.Verify(
                _ => _.SearchRecipes(
                    It.Is<RecipeSearchFilters>(x => x.UserId == "foobaruserid")
                ),
                Times.Once
            );
        }
    }
}
