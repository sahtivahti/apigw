using System.Collections.Generic;
namespace apigw.ExternalServices.BeerCalculator
{
    public class CalculationRequest
    {
        public double BatchSize { get; set; }
        public double BoilSize { get; set; }
        public IEnumerable<Hop> Hops { get; set; }
        public IEnumerable<Fermentable> Fermentables { get; set; }
    }

    public class Hop
    {
        public double Weight { get; set; }
        public double Aa { get; set; }
        public int Time { get; set; }
    }

    public class Fermentable
    {
        public double Weight { get; set; }
        public double Color { get; set; }
    }
}
