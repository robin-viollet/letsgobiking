using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RoutingServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            HttpClient client = new HttpClient();
            List<Double[]> coordinates = new List<Double[]>();
            coordinates.Add(new double[] { 8.681495, 49.41461 });
            coordinates.Add(new double[] { 8.686507, 49.41943 });
            coordinates.Add(new double[] { 8.687872, 49.420318 });

            StringContent content = new StringContent(JsonSerializer.Serialize(new {
                coordinates,
            }), Encoding.UTF8, "application/json");
            Console.WriteLine(content.ReadAsStringAsync().Result);
            
            new HttpRequestMessage(HttpMethod.Post, "https://api.openrouteservice.org/v2/directions/driving-car");

            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://api.openrouteservice.org/v2/directions/driving-car"),
                Headers = {
                            { HttpRequestHeader.Authorization.ToString(), "5b3ce3597851110001cf6248cfa6871d615a44e39df1352fbd201d08" },
                            { HttpRequestHeader.Accept.ToString(), "application/json, application/geo+json, application/gpx+xml, img/png; charset=utf-8" },
                            { HttpRequestHeader.ContentType.ToString(), "application/json; charset=utf-8"}
                },
                Content = content
            };

            HttpResponseMessage response = client.SendAsync(httpRequestMessage).Result;
            String result = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(result);
            Console.ReadLine();
        }
    }
}
