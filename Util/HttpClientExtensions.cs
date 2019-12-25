using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Formatting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace apigw.Util
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> PutJsonAsync<T>(this HttpClient client, string path, T data)
        {
            return await client.PutAsync(path, data, GetFormatter());
        }

        public static async Task<HttpResponseMessage> PostJsonAsync<T>(this HttpClient client, string path, T data)
        {
            return await client.PostAsync(path, data, GetFormatter());
        }

        private static JsonMediaTypeFormatter GetFormatter()
        {
            var formatter = new JsonMediaTypeFormatter();

            formatter.SerializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };

            return formatter;
        }
    }
}
