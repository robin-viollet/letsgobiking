using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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

            return base.SendGetRequest<List<Place>>(keysAndValues: keyValuePairs)[0];
        }
    }
}
