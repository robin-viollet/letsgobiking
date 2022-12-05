
using System.Collections.Generic;

namespace ProxyCache
{
    public class ProxyCacheServicesProvider : IProxyCacheServices
    {
        private static readonly GenericProxyCache genericProxyCache = new GenericProxyCache();

        public List<Contract> GetAllContracts()
        {
            return genericProxyCache.Get<JCDecauxContractsItem>("contracts").GetItemContent();
        }

        public List<Station> GetStationsFromContract(string contractName)
        {
            return genericProxyCache.Get<JCDecauxStationsItem>(contractName).GetItemContent();
        }
    }
}
