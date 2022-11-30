using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace RoutingServer
{
    internal class RequestHelper
    {
        protected readonly HttpClient client;
        protected readonly String api_address;
        protected readonly String apiKeyAndValue = null;

        public RequestHelper(String api_address)
        {
            this.client = new HttpClient
            {
                MaxResponseContentBufferSize = 10000000000
            };
            this.client.DefaultRequestHeaders.Add("User-Agent", "C# App");
            this.api_address = api_address;
        }

        public RequestHelper(String api_address, String apiKey, String apiValue) : this(api_address) {
            this.apiKeyAndValue = apiKey + "=" + apiValue;
        }

        protected T SendGetRequest<T>(String service = "", Dictionary<String, String> keysAndValues = null)
        {
            String request = this.api_address;
            request += "/" + service + "?";

            if(this.apiKeyAndValue != null)
            {
                request += this.apiKeyAndValue + "&";
            }

            if(keysAndValues != null)
            {
                foreach (KeyValuePair<String, String> entry in keysAndValues.ToList())
                {
                    request += entry.Key + "=" + HttpUtility.UrlEncode(entry.Value) + "&";
                }
            }
            Console.WriteLine("Send request : " + request);

            HttpResponseMessage response = this.client.GetAsync(request).Result;

            Console.WriteLine("Status code : " + response.StatusCode);
            response.EnsureSuccessStatusCode();
            
            Console.WriteLine("Response received : " + response.Content.ReadAsStringAsync().Result);

            return JsonSerializer.Deserialize<T>(response.Content.ReadAsStringAsync().Result);
        }

        protected T SendPostRequest<T>(HttpRequestMessage httpRequestMessage)
        {
            HttpResponseMessage response = this.client.SendAsync(httpRequestMessage).Result;

            response.EnsureSuccessStatusCode();

            Console.WriteLine(response.Content.ReadAsStringAsync().Result);

            return JsonSerializer.Deserialize<T>(response.Content.ReadAsStringAsync().Result);
        }
    }
}
