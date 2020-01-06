using System;
using PactNet;
using PactNet.Mocks.MockHttpService;

namespace apigw.Pact
{
    public class BeerCalculatorPact : IDisposable
    {
        public IPactBuilder PactBuilder { get; set; }
        public IMockProviderService MockProviderService { get; set; }
        public string MockProviderServiceBaseUri { get { return String.Format("http://localhost:{0}", 9221); } }

        public BeerCalculatorPact()
        {
            PactBuilder = new PactBuilder();

            PactBuilder
                .ServiceConsumer("API GW")
                .HasPactWith("Beer Calculator");

            MockProviderService = PactBuilder.MockService(9221);
        }

        public void Dispose()
        {
            PactBuilder.Build();
        }
    }
}
