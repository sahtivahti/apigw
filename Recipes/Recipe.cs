using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace apigw.Recipes
{
    public class Recipe
    {
        public int? Id { get; set; }

        [Required]
        [EmailAddress]
        public string Author { get; set; }

        public string UserId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Style { get; set; }

        public double BatchSize { get; set; }

        public DateTime? CreatedAt { get; set; } = null;

        public DateTime? UpdatedAt { get; set; } = null;

        public IEnumerable<Hop> Hops { get; set; }

        public IEnumerable<Fermentable> Fermentables { get; set; }
    }
}
