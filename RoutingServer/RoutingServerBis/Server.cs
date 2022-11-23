using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Text.Json;
using System.Threading.Tasks;

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

    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Service1" à la fois dans le code et le fichier de configuration.
    public class Server : IServices
    {
        Itinerary IServices.GetBestPath(Location startLocation, Location endLocation)
        {
            throw new NotImplementedException();
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
