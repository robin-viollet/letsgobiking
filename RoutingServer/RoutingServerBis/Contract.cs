using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutingServer
{
    public class Contract
    {
        
        public String name { get; set; }
        public String commercial_name { get; set; }
        public String country_code { get; set; }
        public List<string> cities { get; set; }
    }
}
