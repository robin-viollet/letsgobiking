using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Text.Json;

namespace RoutingServer
{
    internal class OpenRouteDirectionService : RequestHelper
    {
        private static readonly String ApiKey = "5b3ce3597851110001cf6248fddc708e42574a909cca16e732eb79e7";

        public OpenRouteDirectionService() : base("https://api.openrouteservice.org/v2/directions/", "api_key", OpenRouteDirectionService.ApiKey)
        { }

        private Itinerary GetItinerary(String startCoordinate, String endCoordinate, string profile)
        {
            Dictionary<String, String> keyValuePairs = new Dictionary<String, String>
            {
                { "start", startCoordinate },
                { "end", endCoordinate },
            };
            return base.SendGetRequest<Itinerary>(profile, keyValuePairs);
        }

        public Itinerary GetFootWalkingItinerary(String startCoordinate, String endCoordinate)
        {
            return this.GetItinerary(startCoordinate, endCoordinate, "foot-walking");
        }

        public Itinerary GetBikingItinerary(String startCoordinate, String endCoordinate)
        {
            return this.GetItinerary(startCoordinate, endCoordinate, "cycling-regular");
        }

        /*private MultipleCheckpointsItinary PostItinerary(List<Double[]> checkpointsCoordinates, string profile)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, base.api_address + profile);
            httpRequestMessage.Headers.Add(HttpRequestHeader.Authorization.ToString(), OpenRouteDirectionService.ApiKey);
            httpRequestMessage.Headers.Add(HttpRequestHeader.Accept.ToString(), "application/json; charset=utf-8");
            httpRequestMessage.Headers.Add(HttpRequestHeader.ContentType.ToString(), "application/json; charset=utf-8");

            StringContent content = new StringContent(JsonSerializer.Serialize(new
            {
                coordinates = checkpointsCoordinates,
            }), Encoding.UTF8, "application/json");
            httpRequestMessage.Content = content;

            return base.SendPostRequest<MultipleCheckpointsItinary>(httpRequestMessage);
        }

        public MultipleCheckpointsItinary GetFootWalkingItinerary(List<Double[]> checkpointsCoordinates)
        {
            return this.PostItinerary(checkpointsCoordinates, "foot-walking");
        }

        public MultipleCheckpointsItinary GetBikingItinerary(List<Double[]> checkpointsCoordinates)
        {
            return this.PostItinerary(checkpointsCoordinates, "cycling-regular");
        }*/
    }
}
