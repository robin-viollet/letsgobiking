using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace RoutingServer
{
    internal class RestClient
    {

        // HttpClient is intended to be instantiated once per application, rather than per-use. See Remarks.
        readonly HttpClient client;

        static String apiKey = "ac8828262f441bebc10d8f59f3aa109399f6f66b";

        public RestClient()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "C# App");
        }

        public async Task<List<Contract>> GetAllContracts()
        {
            HttpResponseMessage response = await this.client.GetAsync("https://api.jcdecaux.com/vls/v3/contracts?apiKey=" + apiKey);

            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<List<Contract>>(responseBody);
        }


        public async Task<List<Station>> GetAllStations(Contract c)
        {
            HttpResponseMessage response = await this.client.GetAsync("https://api.jcdecaux.com/vls/v3/stations?apiKey=" + apiKey + "&contract=" + c.name);

            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<List<Station>>(responseBody);
        }


        public async Task<Place> getPlace(String street, String city, String country, String postalCode)
        {
            street = HttpUtility.UrlEncode(street);
            city = HttpUtility.UrlEncode(city);
            country = HttpUtility.UrlEncode(country);
            postalCode = HttpUtility.UrlEncode(postalCode);

            String url = "https://nominatim.openstreetmap.org/?format=json&limit=1&street=" + street + "&city=" + city + "&country=" + country + "&postalcode=" + postalCode;

            Console.WriteLine(url);

            HttpResponseMessage response = await this.client.GetAsync(url);

            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"Response: {responseBody}");

            return JsonSerializer.Deserialize<List<Place>>(responseBody)[0];
        }
    }
}
