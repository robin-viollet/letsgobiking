using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace ProxyCache
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Service1" à la fois dans le code et le fichier de configuration.
    public class GenericProxyCache
    {
        private readonly Dictionary<Type, MemoryCache> caches = new Dictionary<Type, MemoryCache>();

        private static readonly DateTimeOffset dt_default = ObjectCache.InfiniteAbsoluteExpiration;

        public T Get<T>(string CacheItemName) where T : class
        {
            return this.Get<T>(CacheItemName, dt_default);
        }

        public T Get<T>(string CacheItemName, double dt_seconds) where T : class
        {
            return this.Get<T>(CacheItemName, DateTimeOffset.Now.AddSeconds(dt_seconds));
        }

        public T Get<T>(string CacheItemName, DateTimeOffset dt) where T : class
        {
            Type type = typeof(T);
            if (!this.caches.TryGetValue(type, out MemoryCache cache))
            {
                cache = new MemoryCache(type.Name);
                this.caches.Add(type, cache);
            }

            T result = (T) cache.Get(CacheItemName);

            if (result == null)
            {
                result = (T) Activator.CreateInstance(type, CacheItemName);
            }

            cache.Add(CacheItemName, result, dt);
            
            return result;
        }
    }
}
