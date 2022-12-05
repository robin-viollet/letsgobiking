using RoutingServer.IProxyCacheServices;
using System.Collections.Generic;


namespace RoutingServer
{
    internal class JCDecaux/* : RequestHelper*/
    {
        private static readonly ProxyCacheServicesClient proxyCacheServicesClient = new ProxyCacheServicesClient();

        /*public JCDecaux() : base("https://api.jcdecaux.com/vls/v3/", "apiKey", "ac8828262f441bebc10d8f59f3aa109399f6f66b")
        { }*/

        public List<Contract> GetAllContracts()
        {
            return proxyCacheServicesClient.GetAllContracts();
        }

        public List<Station> GetAllStations(Contract contract)
        {
            return proxyCacheServicesClient.GetStationsFromContract(contract.name);
        }
    }
}
