using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace apigw.Recipes
{
    public class MockRecipeService : IRecipeService
    {
        private IEnumerable<Recipe> recipes = new List<Recipe>
        {
            new Recipe
            {
                Id = 1,
                Name = "My another recipe",
                Author = "panomestari@sahtivahti.fi"
            }
        };

        public Task<Recipe> GetRecipeById(int id)
        {
            return Task.FromResult(recipes.First(x => x.Id == id));
        }

        public Task<IEnumerable<Recipe>> GetRecipes()
        {
            return Task.FromResult(recipes);
        }

        public Task<Recipe> UpdateRecipe(Recipe recipe)
        {
            return Task.FromResult(recipe);
        }

        public Task<Recipe> CreateRecipe(Recipe recipe)
        {
            return Task.FromResult(recipe);
        }
    }
}
