using System;
using System.Collections.Generic;
using apigw.BeerCalculator;

namespace apigw.Recipes
{
    public class RecipeDetailsResponse
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Style { get; set; }
        public double BatchSize { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public IEnumerable<Hop> Hops { get; set; }
        public IEnumerable<Fermentable> Fermentables { get; set; }
        public double OriginalGravity { get; set; }
        public double FinalGravity { get; set; }
        public double Abv { get; set; }
        public double Ibu { get; set; }
        public double Color { get; set; }
        public string ColorName { get; set; }

        public void Apply(Recipe recipe)
        {
            Id = recipe.Id.GetValueOrDefault();
            Style = recipe.Style;
            Name = recipe.Name;
            Author = recipe.Author;
            BatchSize = recipe.BatchSize;
            CreatedAt = recipe.CreatedAt.GetValueOrDefault();
            UpdatedAt = recipe.UpdatedAt.GetValueOrDefault();
            Hops = recipe.Hops;
            Fermentables = recipe.Fermentables;
        }

        public void Apply(CalculationResponse response)
        {
            Abv = response.Abv;
            Color = response.ColorEbc;
            ColorName = response.ColorName;
            Ibu = response.Ibu;
            OriginalGravity = response.Og;
            FinalGravity = response.Fg;
        }
    }
}
