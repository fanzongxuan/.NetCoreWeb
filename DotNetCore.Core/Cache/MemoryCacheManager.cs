using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Core.Cache
{
    public class MemoryCacheManager : ICacheManager
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCacheManager(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void Clear()
        {
            _memoryCache.Dispose();
        }

        public void Dispose()
        {
        }

        public virtual T Get<T>(string key)
        {
            return (T)_memoryCache.Get(key);
        }


        public bool IsSet(string key)
        {
            object value;
            _memoryCache.TryGetValue(key, out value);
            return value != null;
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        public void RemoveByPattern(string pattern)
        {
            throw new NotImplementedException();
        }

        public virtual void Set(string key, object data, int cacheTime)
        {
            if (data == null)
                return;

            var cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(cacheTime));
            _memoryCache.Set(key, data, cacheOptions);
        }
    }
}
