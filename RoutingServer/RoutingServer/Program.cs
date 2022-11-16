using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

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
                
                foreach(Contract c in contracts)
                {
                    Console.WriteLine(c.name);
                }

                Contract selectedContract = contracts.Last<Contract>();

                List<Station> stations = restClient.GetAllStations(selectedContract).Result;
                Console.WriteLine("Nombre de station avec le contrat " + selectedContract.name + " : " + stations.Count);

                foreach(Station station in stations)
                {
                    Console.WriteLine(station.name);
                }

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
        readonly HttpClient client = new HttpClient();

        String apiKey = "ac8828262f441bebc10d8f59f3aa109399f6f66b";

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

        
    }
}
