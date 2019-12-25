using System;
using PactNet;
using PactNet.Mocks.MockHttpService;

namespace apigw.Pact
{
    public class RecipeServicePact : IDisposable
    {
        public IPactBuilder PactBuilder { get; set; }
        public IMockProviderService MockProviderService { get; set; }
        public string MockProviderServiceBaseUri { get { return String.Format("http://localhost:{0}", 9222); } }

        public RecipeServicePact()
        {
            PactBuilder = new PactBuilder();

            PactBuilder
                .ServiceConsumer("API GW")
                .HasPactWith("Recipe Service");

            MockProviderService = PactBuilder.MockService(9222);
        }

        public void Dispose()
        {
            PactBuilder.Build();
        }
    }
}
