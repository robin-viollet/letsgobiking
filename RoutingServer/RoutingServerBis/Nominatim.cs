using System;
using System.Collections.Generic;

namespace RoutingServer
{
    internal class Nominatim : RequestHelper
    {
        public Nominatim() : base("https://nominatim.openstreetmap.org/")
        { }

        public Place GetPlace(Location location)
        {
            Dictionary<String, String> keyValuePairs = new Dictionary<string, string>
            {
                { "format", "json" },
                { "limit", "1" },
                { "street", location.street },
                { "city", location.city },
                { "country", location.country },
                { "postalcode", location.postalCode }
            };

            List<Place> places = base.SendGetRequest<List<Place>>(keysAndValues: keyValuePairs);
            if (places.Count == 0)
            {
                throw new CantFindAddressException("The address " + location.ToString() + " can not be found!");
            }

            return places[0];
        }
    }
}
