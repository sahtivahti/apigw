using System;
using System.ComponentModel.DataAnnotations;

namespace apigw.Recipes
{
    [Serializable]
    public class Hop
    {
        public int? Id { get; set; } = null;

        [Required]
        public string Name { get; set; }

        [Required]
        public double Quantity { get; set; }

        [Required]
        public int Time { get; set; }
    }
}
