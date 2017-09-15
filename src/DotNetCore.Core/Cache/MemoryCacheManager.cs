using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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
            var res = _memoryCache.Get(key);
            if (res != null)
            {
                return (T)_memoryCache.Get(key);
            }
            else
            {
                return default(T);
            }
        }


        public bool IsSet(string key)
        {
            object value;
            return _memoryCache.TryGetValue(key, out value);
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        public void RemoveByPattern(string pattern)
        {
            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            object entries = this._memoryCache.GetType().GetField("_entries", flags).GetValue(this._memoryCache);
            IDictionary cacheItems = entries as IDictionary;
            List<string> keys = new List<string>();
            foreach (var cacheItem in cacheItems.Keys)
            {
                keys.Add(cacheItem.ToString());
            }
            this.RemoveByPattern(pattern, keys);
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
