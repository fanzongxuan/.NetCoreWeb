using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Core.Cache
{
    public class DistributeCacheManager : ICacheManager
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ICacheManager _perRequestCacheManager;

        public DistributeCacheManager(IDistributedCache distributedCache,
            ICacheManager perRequestCacheManager)
        {
            _distributedCache = distributedCache;
            _perRequestCacheManager = perRequestCacheManager;
        }

        protected virtual string Serialize(object item)
        {
            var jsonString = JsonConvert.SerializeObject(item);
            return jsonString;
        }
        protected virtual T Deserialize<T>(string jsonString)
        {
            if (jsonString == null)
                return default(T);

            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        public void Clear()
        {
        }

        public void Dispose()
        {
        }

        public T Get<T>(string key)
        {
            //this way we won't connect to Redis server 500 times per HTTP request (e.g. each time to load a locale or setting)
            if (_perRequestCacheManager.IsSet(key))
                return _perRequestCacheManager.Get<T>(key);

            var rValue = _distributedCache.GetString(key);
            if (string.IsNullOrEmpty(rValue))
                return default(T);
            var result = Deserialize<T>(rValue);

            _perRequestCacheManager.Set(key, result, 0);
            return result;
        }

        public bool IsSet(string key)
        {
            if (_perRequestCacheManager.IsSet(key))
                return true;

            return !string.IsNullOrWhiteSpace(_distributedCache.GetString(key));
        }

        public void Remove(string key)
        {
            _distributedCache.Remove(key);
            _perRequestCacheManager.Remove(key);
        }

        public void RemoveByPattern(string pattern)
        {
                //foreach (var key in keys)
                //    Remove(key);
            
        }

        public void Set(string key, object data, int cacheTime)
        {
            if (data == null)
                return;

            var entryBytes = Serialize(data);
            var expiresIn = TimeSpan.FromMinutes(cacheTime);
            DistributedCacheEntryOptions opt = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = expiresIn
            };
            _distributedCache.SetString(key, entryBytes, opt);
        }
    }
}
