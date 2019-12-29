using System.Collections.Generic;
using System.Threading.Tasks;

namespace apigw.Recipes
{
    public interface IRecipeService
    {
        public Task<IEnumerable<Recipe>> GetRecipes();
        public Task<Recipe> GetRecipeById(int id);
        public Task<Recipe> UpdateRecipe(Recipe recipe);
        public Task<Recipe> CreateRecipe(Recipe recipe);
        public Task RemoveRecipeById(int id);
    }
}
