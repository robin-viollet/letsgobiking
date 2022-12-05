using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProxyCache
{
    public abstract class AbstractCacheItem<T>
    {
        private static readonly HttpClient httpClient = new HttpClient();

        private readonly T itemContent;

        static AbstractCacheItem()
        {
            httpClient.DefaultRequestHeaders.Add("User-Agent", "C# App");
            httpClient.MaxResponseContentBufferSize = int.MaxValue;
        }

        public AbstractCacheItem(String requestUrl)
        {
            HttpResponseMessage response = httpClient.GetAsync(requestUrl).Result;
            this.itemContent = JsonSerializer.Deserialize<T>(response.Content.ReadAsStringAsync().Result);
        }

        public T GetItemContent()
        {
            return itemContent;
        }
    }
}
