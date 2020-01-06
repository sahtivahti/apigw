using System.Collections.Generic;
using System;
using apigw.Recipes;
using Xunit;
using apigw.BeerCalculator;

namespace apigw.Test.Recipes
{
    public class RecipeDetailsResponseTest
    {
        [Fact]
        public void ApplyRecipe_WillApplyProperFields()
        {
            var response = new RecipeDetailsResponse();
            var now = DateTime.UtcNow;

            var recipe = new Recipe
            {
                Id = 1,
                Name = "Recipe",
                BatchSize = 12,
                Style = "IPA",
                Author = "sahti.vahti@sahtivahti.fi",
                CreatedAt = now,
                UpdatedAt = now,
                Hops = new List<apigw.Recipes.Hop>(),
                Fermentables = new List<apigw.Recipes.Fermentable>()
            };

            response.Apply(recipe);

            Assert.Equal(recipe.Id, response.Id);
            Assert.Equal(recipe.Name, response.Name);
            Assert.Equal(recipe.BatchSize, response.BatchSize);
            Assert.Equal(recipe.Style, response.Style);
            Assert.Equal(recipe.Author, response.Author);
            Assert.Equal(recipe.CreatedAt, response.CreatedAt);
            Assert.Equal(recipe.UpdatedAt, response.UpdatedAt);
            Assert.Equal(recipe.Hops, response.Hops);
            Assert.Equal(recipe.Fermentables, response.Fermentables);
        }

        [Fact]
        public void ApplyCalculationResponse_WillApplyProperFields()
        {
            var response = new RecipeDetailsResponse();

            var calculationResponse = new CalculationResponse
            {
                Og = 1.06,
                Fg = 1.01,
                Abv = 6.7,
                ColorName = "gold",
                ColorEbc = 10,
                ColorSrm = 6,
                Ibu = 60
            };

            response.Apply(calculationResponse);

            Assert.Equal(calculationResponse.Og, response.OriginalGravity);
            Assert.Equal(calculationResponse.Fg, response.FinalGravity);
            Assert.Equal(calculationResponse.Abv, response.Abv);
            Assert.Equal(calculationResponse.ColorEbc, response.Color);
            Assert.Equal(calculationResponse.ColorName, response.ColorName);
            Assert.Equal(calculationResponse.Ibu, response.Ibu);
        }
    }
}
