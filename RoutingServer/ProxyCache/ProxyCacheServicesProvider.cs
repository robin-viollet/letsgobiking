
using System.Collections.Generic;

namespace ProxyCache
{
    public class ProxyCacheServicesProvider : IProxyCacheServices
    {
        private readonly GenericProxyCache genericProxyCache = new GenericProxyCache();

        public List<Station> GetStationsFromContract(string contractName)
        {
            return this.genericProxyCache.Get<JCDecauxStationsItem>(contractName).GetItemContent();
        }
    }
}
