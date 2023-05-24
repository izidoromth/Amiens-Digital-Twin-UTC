using System.Net.Http;
using System.Threading;
using Newtonsoft.Json;

namespace APIRequest
{
    public class HttpClientService
    {
        private readonly HttpClient httpClient = new HttpClient();
        public T Get<T>(string route)
        {
            using HttpClient httpClient = this.httpClient;
            var response = httpClient.GetAsync(route).Result;
            return Read<T>(response);
        }

        protected T Read<T>(HttpResponseMessage response)
        {
            return ReadByType<T>(response);
        }

        private T ReadByType<T>(HttpResponseMessage response)
        {
            return ReadAsJson<T>(response);
        }

        private T ReadAsJson<T>(HttpResponseMessage response)
        {
            var content = response.Content.ReadAsStringAsync().Result;
            return Deserialize<T>(content);
        }

        public T Deserialize<T>(string content)
        {
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}
