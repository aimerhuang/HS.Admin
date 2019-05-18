using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text.RegularExpressions;

namespace Hyt.Infrastructure.Memory
{
    /// <summary>
    /// MemoryCache缓存
    /// </summary>
    /// <remarks>2013-8-2 杨浩 添加</remarks>
    public class MemoryCacheProvider : ICache
    {

        protected ObjectCache Cache
        {
            get { return MemoryCache.Default; }
        }

        /// <summary>
        ///根据键获取值
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">键</param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            return (T) Cache[key];
        }

        public object Get(string key)
        {
            return Cache[key];
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="cacheTime">绝对过期时间(单位/分)</param>
        /// <param name="cachePolicy">缓存策略</param>
        public void Set(string key, object value, int cacheTime, CachePolicy cachePolicy = CachePolicy.Absolute)
        {
            if (value == null)
                return;

            var policy = new CacheItemPolicy();
            if (cachePolicy == CachePolicy.Absolute)
                policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime);
            else
                policy.SlidingExpiration = TimeSpan.FromMinutes(cacheTime);
            Cache.Add(new CacheItem(key, value), policy);
        }

        /// <summary>
        /// 是否存在缓存项
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            return (Cache.Contains(key));
        }

        /// <summary>
        /// 移除缓存项
        /// </summary>
        /// <param name="key">key</param>
        public void 
            Remove(string key)
        {
            Cache.Remove(key);
        }

        /// <summary>
        /// 根据匹配符移除缓存项
        /// </summary>
        /// <param name="pattern">pattern</param>
        public void RemoveByPattern(string pattern)
        {
            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var keysToRemove = new List<String>();

            foreach (var item in Cache)
                if (regex.IsMatch(item.Key))
                    keysToRemove.Add(item.Key);

            foreach (string key in keysToRemove)
            {
                Remove(key);
            }
        }

        /// <summary>
        ///移除所有缓存
        /// </summary>
        public void Clear()
        {
            foreach (var item in Cache)
                Remove(item.Key);
        }

        /// <summary>
        ///获取所有key
        /// </summary>
        public IList<string> GetAllKey()
        {
            var keys = Cache.Where(x => x.Key.Contains(KeyConstant.Prefix))
                            .Select(item => item.Key)
                            .ToList();
            return keys;
        }
    }
}
