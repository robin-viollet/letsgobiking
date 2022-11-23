using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutingServer
{
    public class Place
    {
        public Address address { get; set; }
        public List<string> boundingbox { get; set; }
        public string @class { get; set; }
        public string display_name { get; set; }
        public string icon { get; set; }
        public double importance { get; set; }
        public string lat { get; set; }
        public string licence { get; set; }
        public string lon { get; set; }
        public int osm_id { get; set; }
        public string osm_type { get; set; }
        public int place_id { get; set; }
        public string type { get; set; }
    }

    public class Address
    {
        public string bakery { get; set; }
        public string city_district { get; set; }
        public string continent { get; set; }
        public string country { get; set; }
        public string country_code { get; set; }
        public string footway { get; set; }
        public string neighbourhood { get; set; }
        public string postcode { get; set; }
        public string state { get; set; }
        public string suburb { get; set; }
    }
}
