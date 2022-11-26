using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Text.Json;
using System.Threading.Tasks;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Device.Location;
using System.Net;
using System.Text.Json.Serialization;

namespace RoutingServer
{
    /*internal class Program
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
    }*/

    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Service1" à la fois dans le code et le fichier de configuration.
    public class Server : IServices
    {
        private JCDecaux JCDecaux;
        private Nominatim Nominatim;
        private OpenRouteDirectionService OpenRouteDirectionService;

        public Server()
        {
            this.JCDecaux = new JCDecaux();
            this.Nominatim = new Nominatim();
            this.OpenRouteDirectionService= new OpenRouteDirectionService();
        }

        Itinerary IServices.GetBestPath(Location startLocation, Location endLocation)
        {
            /*HttpClient client = new HttpClient();
            Dictionary<String, String> keyValuePairs = new Dictionary<String, String>
            {
                {"coordinates", "[[8.681495,49.41461],[8.686507,49.41943],[8.687872,49.420318]]" },
            };
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://api.openrouteservice.org/v2/directions/driving-car"),
                Headers = {
                            { HttpRequestHeader.Authorization.ToString(), "Bearer 5b3ce3597851110001cf6248cfa6871d615a44e39df1352fbd201d08" },
                            { HttpRequestHeader.Accept.ToString(), "application/json, application/geo+json, application/gpx+xml, img/png; charset=utf-8" },
                            { HttpRequestHeader.ContentType.ToString(), "application/json; charset=utf-8"}
                },
            };

            httpRequestMessage.Content = new FormUrlEncodedContent(keyValuePairs);

            HttpResponseMessage response = client.SendAsync(httpRequestMessage).Result;
            String result = response.Content.ReadAsStringAsync().Result;

            Itinerary toSendBack = new Itinerary();
            toSendBack.type = result;

            return toSendBack;*/

            List<Contract> contracts = this.JCDecaux.GetAllContracts();
            Contract concernedContract = this.GetConcernedContract(startLocation, contracts);

            List<Station> stationOfTheContract = this.JCDecaux.GetAllStations(concernedContract);

            GeoCoordinate startCoordinate = this.GetGeoCoordinate(startLocation);
            GeoCoordinate endCoordinate = this.GetGeoCoordinate(endLocation);

            Station pickUpStation = this.GetClosestPickUpStation(startCoordinate, stationOfTheContract);
            Station dropOffStation = this.GetClosestDropOffStation(endCoordinate, stationOfTheContract);


            throw new NotImplementedException();
        }

        private Contract GetConcernedContract(Location location, List<Contract> contracts)
        {
            foreach (Contract contract in contracts)
            {
                if (contract.cities.Contains(location.city))
                {
                    return contract;
                }
            }
            //FIXME
            return null;
        }

        private Station GetClosestPickUpStation(GeoCoordinate geoCoordinate, List<Station> stations)
        {
            return this.GetClosestStation(geoCoordinate, stations.Where(s => s.CanPickUpABike()).ToList());
        }

        private Station GetClosestDropOffStation(GeoCoordinate geoCoordinate, List<Station> stations)
        {
            return this.GetClosestStation(geoCoordinate, stations.Where(s => s.CanDropOffABike()).ToList());
        }

        private Station GetClosestStation(GeoCoordinate geoCoordinate, List<Station> stations)
        {
            Station closestStation = null;
            double closestDistance = Double.MaxValue;

            GeoCoordinate toCompare;
            double temporaryDistance;
            foreach (Station station in stations)
            {
                toCompare = new GeoCoordinate(station.position.latitude, station.position.longitude);
                temporaryDistance = geoCoordinate.GetDistanceTo(toCompare);
                if (temporaryDistance <= closestDistance)
                {
                    closestStation = station;
                    closestDistance = temporaryDistance;
                }
            }
            return closestStation;
        }

        private GeoCoordinate GetGeoCoordinate(Location location)
        {
            Place place = this.Nominatim.GetPlace(location);
            return new GeoCoordinate(Double.Parse(place.lat), Double.Parse(place.lon));
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
