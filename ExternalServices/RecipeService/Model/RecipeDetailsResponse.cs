using System.Collections.Generic;
using System;

namespace apigw.ExternalServices.RecipeService.Model
{
    public class RecipeDetailsResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string UserId { get; set; }
        public string Style { get; set; }
        public double BatchSize { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public IEnumerable<HopDetailsResponse> Hops { get; set; } = new List<HopDetailsResponse>();
        public IEnumerable<FermentableDetailsResponse> Fermentables { get; set; } = new List<FermentableDetailsResponse>();
    }
}
