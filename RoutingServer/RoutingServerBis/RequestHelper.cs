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
        private readonly HttpClient client;
        private readonly String api_address;
        private readonly String apiKeyAndValue = null;

        public RequestHelper(String api_address)
        {
            this.client = new HttpClient();
            this.client.DefaultRequestHeaders.Add("User-Agent", "C# App");
            this.api_address = api_address;
        }

        public RequestHelper(String api_address, String apiKey, String apiValue) : this(api_address) {
            this.apiKeyAndValue = apiKey + "=" + apiValue;
        }

        public T SendRequest<T>(String service = "", Dictionary<String, String> keysAndValues = null)
        {
            String request = this.api_address;
            request += "/" + service + "?";

            if(this.apiKeyAndValue != null)
            {
                request += this.apiKeyAndValue;
            }

            if(keysAndValues!= null)
            {
                foreach (KeyValuePair<String, String> entry in keysAndValues.ToList())
                {
                    request += entry.Key + "=" + HttpUtility.UrlEncode(entry.Value) + "&";
                }
            }

            HttpResponseMessage response = this.client.GetAsync(request).Result;

            response.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<T>(response.Content.ReadAsStringAsync().Result);
        }
    }
}
