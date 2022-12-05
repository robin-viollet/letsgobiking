using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Web;

namespace RoutingServer
{
    internal class RequestHelper
    {
        protected static readonly HttpClient client;
        protected readonly String api_address;
        protected readonly String apiKeyAndValue = null;

        static RequestHelper()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "C# App");
            client.MaxResponseContentBufferSize = int.MaxValue;
            
        }

        public RequestHelper(String api_address)
        {
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

            HttpResponseMessage response = client.GetAsync(request).Result;

            Console.WriteLine("Status code : " + response.StatusCode);
            response.EnsureSuccessStatusCode();
            
            Console.WriteLine("Response received : " + response.Content.ReadAsStringAsync().Result);

            return JsonSerializer.Deserialize<T>(response.Content.ReadAsStringAsync().Result);
        }

        protected T SendPostRequest<T>(HttpRequestMessage httpRequestMessage)
        {
            HttpResponseMessage response = client.SendAsync(httpRequestMessage).Result;

            response.EnsureSuccessStatusCode();

            Console.WriteLine(response.Content.ReadAsStringAsync().Result);

            return JsonSerializer.Deserialize<T>(response.Content.ReadAsStringAsync().Result);
        }
    }
}
