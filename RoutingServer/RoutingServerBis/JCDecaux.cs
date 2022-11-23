using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RoutingServer
{
    internal class JCDecaux : RequestHelper
    {
        public JCDecaux() : base("https://api.jcdecaux.com/vls/v3/", "apiKey", "ac8828262f441bebc10d8f59f3aa109399f6f66b")
        { }

        public List<Contract> GetAllContracts()
        {
            return base.SendRequest<List<Contract>>("contracts");
        }

        public List<Station> GetAllStations(Contract contract)
        {
            Dictionary<String, String> keyValuePairs = new Dictionary<string, string>
            {
                { "contract", contract.name }
            };
            return base.SendRequest<List<Station>>("stations", keyValuePairs);
        }
    }
}
