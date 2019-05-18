using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Infrastructure.Memory
{
    /// <summary>
    /// 缓存接口扩展(包装)
    /// </summary>
    /// <remarks>2013-8-2 杨浩 添加</remarks>
    public static class Extensions
    {
        /// <summary>
        /// 获取或设置缓存(默认60分)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheManager"></param>
        /// <param name="key"></param>
        /// <param name="acquire"></param>
        /// <param name="cachePolicy">缓存策略</param>
        /// <returns>T</returns>
        /// <remarks>2013-8-2 杨浩 添加</remarks>
        public static T Get<T>(this ICache cacheManager, string key, Func<T> acquire, CachePolicy cachePolicy = CachePolicy.Absolute)
        {
            return Get(cacheManager, key, 60, acquire, cachePolicy);
        }

        /// <summary>
        /// 获取或设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheManager"></param>
        /// <param name="key"></param>
        /// <param name="cacheTime">分</param>
        /// <param name="acquire">回调方法</param>
        /// <param name="cachePolicy">缓存策略</param>
        /// <returns>T</returns>
        /// <remarks>2013-8-2 杨浩 添加</remarks>
        public static T Get<T>(this ICache cacheManager, string key, int cacheTime, Func<T> acquire, CachePolicy cachePolicy = CachePolicy.Absolute)
        {
            if (cacheManager.Exists(key))
            {
                return cacheManager.Get<T>(key);
            }
            else
            {
                var result = acquire();
                if (result is string && result.ToString()=="")
                    return result;
                cacheManager.Set(key, result, cacheTime, cachePolicy);
                return result;
            }
        }
    }
}
