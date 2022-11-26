using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutingServer
{
    internal class OpenRouteDirectionService : RequestHelper
    {
        public OpenRouteDirectionService() : base("https://api.openrouteservice.org/v2/directions/", "api_key", "5b3ce3597851110001cf6248cfa6871d615a44e39df1352fbd201d08")
        { }

        private Itinerary GetItinerary(String startCoordinate, String endCoordinate, string profile)
        {
            Dictionary<String, String> keyValuePairs = new Dictionary<String, String>
            {
                { "start", startCoordinate },
                { "end", endCoordinate },
            };
            return base.SendRequest<Itinerary>(profile, keyValuePairs);
        }

        private Itinerary PostItinerary(String[] coordinates, string profile)
        {
            Dictionary<String, String> keyValuePairs = new Dictionary<String, String>
            {
                {"Authorization", "5b3ce3597851110001cf6248cfa6871d615a44e39df1352fbd201d08" },
            };



            for(int i = 0 ; i < coordinates.Length ; i++)
            {

            }
            return base.SendRequest<Itinerary>(profile, keyValuePairs);
        }

        public Itinerary GetFootWalkingItinerary(String startCoordinate, String endCoordinate)
        {
            return this.GetItinerary(startCoordinate, endCoordinate, "foot-walking");
        }

        public Itinerary GetCyclingItinerary(String startCoordinate, String endCoordinate)
        {
            return this.GetItinerary(startCoordinate, endCoordinate, "cycling-regular");
        }
    }
}
