using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System;
using System.Net;
using System.Reflection;

namespace DotNetCore.Core.Cache
{
    public class RedisConnectionWrapper : IRedisConnectionWrapper
    {
        private readonly IDistributedCache _distributedCache;
        private volatile ConnectionMultiplexer _connection;

        public RedisConnectionWrapper(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        #region Utilities

        protected string GetConnectionString()
        {
            return GetConnection().Configuration;
        }

        /// <summary>
        /// Get connection to Redis servers
        /// </summary>
        /// <returns></returns>
        private ConnectionMultiplexer GetConnection()
        {
            if (_connection != null && _connection.IsConnected) return _connection;
            
            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var filed = this._distributedCache.GetType().GetField("_connection", flags);
            var entries = filed.GetValue(_distributedCache);
            ConnectionMultiplexer connection = entries as ConnectionMultiplexer;
            _connection = connection;
            return _connection;
        }
        

        #endregion

        #region Methods

        /// <summary>
        /// Obtain an interactive connection to a database inside redis
        /// </summary>
        /// <param name="db">Database number; pass null to use the default value</param>
        /// <returns>Redis cache database</returns>
        public IDatabase GetDatabase(int? db = null)
        {
            return GetConnection().GetDatabase(db ?? -1); //_settings.DefaultDb);
        }

        /// <summary>
        /// Obtain a configuration API for an individual server
        /// </summary>
        /// <param name="endPoint">The network endpoint</param>
        /// <returns>Redis server</returns>
        public IServer GetServer(EndPoint endPoint)
        {
            return GetConnection().GetServer(endPoint);
        }

        /// <summary>
        /// Gets all endpoints defined on the server
        /// </summary>
        /// <returns>Array of endpoints</returns>
        public EndPoint[] GetEndPoints()
        {
            return GetConnection().GetEndPoints();
        }

        /// <summary>
        /// Delete all the keys of the database
        /// </summary>
        /// <param name="db">Database number; pass null to use the default value<</param>
        public void FlushDatabase(int? db = null)
        {
            var endPoints = GetEndPoints();

            foreach (var endPoint in endPoints)
            {
                GetServer(endPoint).FlushDatabase(db ?? -1); //_settings.DefaultDb);
            }
        }

        /// <summary>
        /// Perform some action with Redis distributed lock
        /// </summary>
        /// <param name="resource">The thing we are locking on</param>
        /// <param name="expirationTime">The time after which the lock will automatically be expired by Redis</param>
        /// <param name="action">Action to be performed with locking</param>
        /// <returns>True if lock was acquired and action was performed; otherwise false</returns>
        public bool PerformActionWithLock(string resource, TimeSpan expirationTime, Action action)
        {
            return false;
        }

        /// <summary>
        /// Release all resources associated with this object
        /// </summary>
        public void Dispose()
        {
            //dispose ConnectionMultiplexer
            if (_connection != null)
                _connection.Dispose();
            
        }

        #endregion
    }
}
