using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;

namespace Hyt.Infrastructure.Caching
{
    /// <summary>
    /// 默认的缓存策略，实现了缓存策略接口
    /// </summary>
    /// 杨浩
    /// <remarks>2013-08-01 黄波 重构</remarks>
    public class NetCache : ICache
    {
        private static readonly Cache _cache;
        /// <summary>
        /// 单例缓存对象
        /// </summary>
        static NetCache()
        {
            HttpContext context = HttpContext.Current;
            if (context != null)
            {
                _cache = context.Cache;
            }
            else
            {
                _cache = HttpRuntime.Cache;
            }           
        }

        #region Icache 实现

        public void Set(string key, object value)
        {
            _cache.Add(key, value, null, DateTime.Now.Add(new TimeSpan(0, 10, 0)), TimeSpan.Zero, CacheItemPriority.NotRemovable, null);
        }

        public void Set(string key, object value, DateTime expiry)
        {
            _cache.Add(key, value, null, expiry, TimeSpan.Zero, CacheItemPriority.NotRemovable, null);
        }

        public T Get<T>(string key)
        {
            return (T)_cache[key];
        }

        public void Update(string key, object value)
        {
            _cache.Insert(key, value);
        }

        public void Update(string key, object value, DateTime expiry)
        {
            _cache.Insert(key, value, null, expiry, TimeSpan.Zero);
        }

        public bool Delete(string key)
        {
            return _cache.Remove(key) == null ? false : true;
        }

        public void RemovAllCache()
        {
            var CacheEnum = _cache.GetEnumerator();           
            while (CacheEnum.MoveNext())
            {
                _cache.Remove(CacheEnum.Key.ToString());
            }           
        }
        public List<string> GetAllKeys()
        {
            List<string> keys = new List<string>();
            var _k = _cache.GetEnumerator();
            while (_k.MoveNext())
            {
                keys.Add(_k.Key.ToString());
            }
            return keys;
        }
        public bool IsExists(string key)
        {
            return (_cache[key] != null);
        }

        public bool DeleteByPrefix(string prefix)
        {
            throw new NotImplementedException();
        }
        
        #endregion  
    }
}