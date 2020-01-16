using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using apigw.ExternalServices.RecipeService;
using apigw.Recipes.Model;
using apigw.ExternalServices.RecipeService.Model;
using apigw.ExternalServices.BeerCalculator;
using apigw.Cache;

namespace apigw.Recipes
{
    public class RecipeService : IRecipeService
    {
        private readonly IRecipeServiceClient _recipeServiceClient;
        private readonly IBeerCalculator _beerCalculator;
        private readonly ICache<RecipeDetails> _recipeDetailsCache;

        public RecipeService(
            IRecipeServiceClient recipeServiceClient,
            IBeerCalculator beerCalculator,
            ICache<RecipeDetails> recipeDetailsCache)
        {
            _recipeServiceClient = recipeServiceClient;
            _beerCalculator = beerCalculator;
            _recipeDetailsCache = recipeDetailsCache;
        }

        public async Task<Fermentable> AddFermentableToRecipe(Fermentable fermentable, int recipeId)
        {
            var request = new CreateFermentableRequest
            {
                Name = fermentable.Name,
                Color = fermentable.Color,
                Quantity = fermentable.Quantity
            };

            var result = await _recipeServiceClient.AddFermentableById(recipeId, request);

            return new Fermentable
            {
                Id = result.Id,
                Name = result.Name,
                Color = result.Color,
                Quantity = result.Quantity
            };
        }

        public async Task<Hop> AddHopToRecipe(Hop hop, int recipeId)
        {
            var request = new CreateHopRequest
            {
                Name = hop.Name,
                Quantity = hop.Quantity,
                Time = hop.Time
            };

            var result = await _recipeServiceClient.AddHopById(recipeId, request);

            return new Hop
            {
                Id = result.Id,
                Name = result.Name,
                Quantity = result.Quantity,
                Time = result.Time
            };
        }

        public async Task<Recipe> CreateRecipe(Recipe recipe)
        {
            var request = new CreateRecipeRequest
            {
                Name = recipe.Name,
                Author = recipe.Author,
                UserId = recipe.UserId,
                Style = recipe.Style,
                BatchSize = recipe.BatchSize
            };

            var result = await _recipeServiceClient.Create(request);

            return new Recipe
            {
                Id = result.Id,
                Name = result.Name,
                Author = result.Author,
                UserId = result.UserId,
                Style = result.Style,
                BatchSize = result.BatchSize,
                CreatedAt = result.CreatedAt,
                UpdatedAt = result.UpdatedAt
            };
        }

        public async Task<RecipeDetails> GetCachedRecipeById(int id)
        {
            var cacheResult = await _recipeDetailsCache.Get(id.ToString(), async _ =>
            {
                return await GetRecipeById(id);
            });

            return cacheResult.Value;
        }

        public async Task<RecipeDetails> GetRecipeById(int id)
        {
            var result = await _recipeServiceClient.GetById(id);

            var meta = await _beerCalculator.Calculate(new CalculationRequest
            {
                BatchSize = result.BatchSize,
                BoilSize = 1.2 * result.BatchSize,
                Hops = result.Hops.Select(hop => new ExternalServices.BeerCalculator.Hop
                {
                    Weight = hop.Quantity / 1000,
                    Aa = 5.0,
                    Time = hop.Time
                }),
                Fermentables = result.Fermentables.Select(f => new ExternalServices.BeerCalculator.Fermentable
                {
                    Weight = f.Quantity,
                    Color = f.Color
                })
            });

            return new RecipeDetails
            {
                Id = result.Id,
                Name = result.Name,
                Author = result.Author,
                Style = result.Style,
                BatchSize = result.BatchSize,
                CreatedAt = result.CreatedAt,
                UpdatedAt = result.UpdatedAt.GetValueOrDefault(),
                UserId = result.UserId,
                Hops = result.Hops.Select(h => new Hop
                {
                    Id = h.Id,
                    Name = h.Name,
                    Quantity = h.Quantity,
                    Time = h.Time
                }).ToArray(),
                Fermentables = result.Fermentables.Select(f => new Fermentable
                {
                    Id = f.Id,
                    Name = f.Name,
                    Quantity = f.Quantity,
                    Color = f.Color
                }).ToArray(),
                Abv = meta.Abv,
                FinalGravity = meta.Fg,
                OriginalGravity = meta.Og,
                Color = meta.ColorEbc,
                ColorName = meta.ColorName,
                Ibu = meta.Ibu
            };
        }

        public async Task<IEnumerable<Model.RecipeListItem>> GetRecipesForUser(string userId)
        {
            var recipes = await _recipeServiceClient.SearchRecipes(
                new RecipeSearchFilters
                {
                    UserId = userId
                }
            );

            return recipes.Select(x => new Model.RecipeListItem
            {
                Id = x.Id,
                Name = x.Name
            });
        }

        public async Task RemoveFermentableFromRecipe(int fermentableId, int recipeId)
        {
            await _recipeServiceClient.DeleteFermentableById(recipeId, fermentableId);
        }

        public async Task RemoveHopFromRecipe(int hopId, int recipeId)
        {
            await _recipeServiceClient.DeleteHopById(recipeId, hopId);
        }

        public async Task RemoveRecipeById(int id)
        {
            await _recipeServiceClient.DeleteById(id);

            await _recipeDetailsCache.Clear(id.ToString());
        }

        public async Task<Recipe> UpdateRecipe(Recipe recipe)
        {
            var recipeId = recipe.Id ?? throw new InvalidOperationException("Can't update recipe without id");

            var request = new UpdateRecipeRequest
            {
                Name = recipe.Name,
                Author = recipe.Author,
                Style = recipe.Style,
                BatchSize = recipe.BatchSize
            };

            var result = await _recipeServiceClient.UpdateById(recipeId, request);

            await _recipeDetailsCache.Clear(recipeId.ToString());

            return new Recipe
            {
                Id = result.Id,
                Name = result.Name,
                Author = result.Author,
                Style = recipe.Style,
                BatchSize = recipe.BatchSize,
                Hops = result.Hops.Select(h => new Hop
                {
                    Id = h.Id,
                    Name = h.Name,
                    Quantity = h.Quantity,
                    Time = h.Time
                }),
                Fermentables = result.Fermentables.Select(f => new Fermentable
                {
                    Id = f.Id,
                    Name = f.Name,
                    Quantity = f.Quantity,
                    Color = f.Color
                })
            };
        }
    }
}
