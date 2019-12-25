using System.Collections.Generic;
using System.Threading.Tasks;

namespace apigw.Recipes
{
    public interface IRecipeService
    {
        public Task<IEnumerable<Recipe>> GetRecipes();
        public Task<Recipe> GetRecipeById(int id);

        public Task<Recipe> UpdateRecipe(Recipe recipe);
    }
}
