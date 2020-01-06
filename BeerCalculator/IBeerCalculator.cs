using System.Threading.Tasks;
using apigw.Recipes;

namespace apigw.BeerCalculator
{
    public interface IBeerCalculator
    {
        Task<CalculationResponse> GetForRecipe(Recipe recipe);
    }
}
