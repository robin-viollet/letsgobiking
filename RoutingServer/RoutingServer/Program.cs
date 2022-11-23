using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Web;

namespace RoutingServer
{

    internal class Program
    {
        static void Main(string[] args)
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                RestClient restClient = new RestClient();

                List<Contract> contracts = restClient.GetAllContracts().Result;
                Console.WriteLine("Nombre de contrat : " + contracts.Count);

                foreach (Contract c in contracts)
                {
                    Console.WriteLine("Contract name : " + c.name + "\nCities : ");
                    if (c.cities == null) continue;
                    for (int i = 0; i < c.cities.Length; i++)
                    {
                        Console.WriteLine(c.cities[i]);
                    }
                    Console.WriteLine("\n");
                }

                Console.ReadLine();

                Contract selectedContract = contracts.Last<Contract>();

                List<Station> stations = restClient.GetAllStations(selectedContract).Result;
                Console.WriteLine("Nombre de station avec le contrat " + selectedContract.name + " : " + stations.Count);

                foreach (Station station in stations)
                {
                    Console.WriteLine(station.name);
                }

                Console.ReadLine();

                Place place = restClient.getPlace("16 bis rue victor rougier", "Le Beausset", "France", "83330").Result;
                Console.WriteLine("Longitude = " + place.lon + " | Latitude = " + place.lat);

                Console.ReadLine();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }
    }

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
            street = System.Web.HttpUtility.UrlEncode(street);
            city = System.Web.HttpUtility.UrlEncode(city);
            country = System.Web.HttpUtility.UrlEncode(country);
            postalCode = System.Web.HttpUtility.UrlEncode(postalCode);

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
