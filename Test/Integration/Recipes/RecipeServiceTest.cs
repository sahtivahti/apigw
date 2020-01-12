using System.Threading.Tasks;
using apigw.ExternalServices.BeerCalculator;
using apigw.ExternalServices.RecipeService;
using apigw.ExternalServices.RecipeService.Model;
using apigw.Recipes;
using apigw.Recipes.Model;
using EasyCaching.Core;
using Moq;
using Xunit;

namespace apigw.Test.Integration.Recipes
{
    public class RecipeServiceTest
    {
        private readonly Mock<IRecipeServiceClient> _recipeServiceClientMock;
        private readonly Mock<IBeerCalculator> _beerCalculatorMock;
        private readonly Mock<IEasyCachingProviderFactory> _cacheProviderFactory;
        private readonly Mock<IEasyCachingProvider> _cacheMock;

        public RecipeServiceTest()
        {
            _recipeServiceClientMock = new Mock<IRecipeServiceClient>();
            _beerCalculatorMock = new Mock<IBeerCalculator>();
            _cacheProviderFactory = new Mock<IEasyCachingProviderFactory>();
            _cacheMock = new Mock<IEasyCachingProvider>();
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
                    Author = "sahti.vahti@sahtivahti.fi"
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

            _cacheProviderFactory.Setup(_ => _.GetCachingProvider(It.IsAny<string>()))
                .Returns(_cacheMock.Object);

            _cacheMock.Setup(_ => _.GetAsync<RecipeDetails>(It.IsAny<string>()))
                .Returns(Task.FromResult(new CacheValue<RecipeDetails>(null, false)));

            var recipeService = new RecipeService(
                _recipeServiceClientMock.Object,
                _beerCalculatorMock.Object,
                _cacheProviderFactory.Object
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
        }
    }
}
