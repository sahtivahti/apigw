using System;
using System.Collections.Generic;

namespace apigw.Recipes.Model
{
    [Serializable]
    public class RecipeDetails
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
    }
}
