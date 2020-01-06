namespace apigw.BeerCalculator
{
    public class CalculationResponse
    {
        public double Og { get; set; }
        public double Fg { get; set; }
        public double Abv { get; set; }
        public double Ibu { get; set; }
        public double ColorSrm { get; set; }
        public double ColorEbc { get; set; }
        public string ColorName { get; set; }
    }
}
