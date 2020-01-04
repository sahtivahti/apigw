using System.Threading;
using System;
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
                Author = "panomestari@sahtivahti.fi",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        public Task<Recipe> GetRecipeById(int id)
        {
            var recipe = recipes.FirstOrDefault(x => x.Id == id);

            if (recipe == null)
            {
                throw new RecipeNotFoundException();
            }

            return Task.FromResult(recipe);
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

        public Task RemoveRecipeById(int id)
        {
            return Task.Factory.StartNew(() => Thread.Sleep(100));
        }

        public Task<Hop> AddHopToRecipe(Hop hop, int recipeId)
        {
            return Task.FromResult(hop);
        }

        public Task<Hop> RemoveHopFromRecipe(int hopId, int recipeId)
        {
            return Task.FromResult(new Hop());
        }
    }
}
