using System;
using System.Configuration;

namespace Hyt.Infrastructure.Caching
{
    /// <summary>
    /// 缓存策略管理
    /// </summary>
    /// 杨浩
    /// <remarks>2013-08-01 黄波 重构</remarks>
    public class CacheManager
    {
        private static ICache ICache;

        #region 缓存类型
        /// <summary>
        /// 缓存类型
        /// </summary>
        private enum _CacheType
        {
            /// <summary>
            /// 默认缓存
            /// </summary>
            DefaultCache = 1,
            /// <summary>
            /// 分布式缓存
            /// </summary>
            Memcached = 2
        }
        #endregion

        #region 缓存状态
        /// <summary>
        /// 缓存状态
        /// </summary>
        private enum _CacheState
        {
            /// <summary>
            /// 打开
            /// </summary>
            Open = 1,
            /// <summary>
            /// 关闭
            /// </summary>
            Close = 0
        }
        #endregion

        #region 获取缓存配置
        /// <summary>
        /// 缓存是否开启
        /// </summary>
        public static string CacheState
        {
            get { return ConfigurationManager.AppSettings["CacheState"]; }
        }

        /// <summary>
        /// 获取缓存实例类型
        /// </summary>
        public static string CacheType
        {
            get { return ConfigurationManager.AppSettings["CacheType"]; }
        }

        /// <summary>
        /// 获取缓存默认过期时间
        /// </summary>
        public static int CacheExpiry
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["CacheExpiry"]); }
        }

        #endregion

        /// <summary>
        /// 返回缓存对象
        /// </summary>
        /// <returns>缓存实例</returns>
        public static ICache Instance
        {
            get
            {
                if (_CacheState.Open.ToString() != CacheState)
                {
                    //throw new Exception("缓存被关闭,请开启缓存开关!");
                    return null;
                }
                if (ICache == null)
                {
                    if (_CacheType.Memcached.ToString() == CacheType)
                        ICache = new Memcached();
                    else
                        ICache = new NetCache();
                    return ICache;
                }
                else
                    return ICache;
            }
        }

        /// <summary>
        /// 移除缓存
        /// <param name="prefixKey">前缀关键字</param>
        /// <param name="suffix">关键字后接字符串</param>
        /// </summary>
        /// <remarks>2013-12-3 黄波 创建</remarks>
        public static void RemoveCache(CacheKeys.Items key, string suffix = "")
        {
            //缓存关键字
            var cacheKey = key.ToString();
            if (!string.IsNullOrEmpty(suffix))
            {
                cacheKey += suffix;
            }

            if (CacheManager.Instance.IsExists(cacheKey))
            {
                CacheManager.Instance.Delete(cacheKey);
            }
        }

        /// <summary>
        /// 缓存代理
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="key">枚举类型关键字</param>
        /// <param name="acquire">方法变量</param>
        /// <param name="methodOptions">操作类型</param>
        /// <returns>缓存数据</returns>
        /// <remarks>2013-08-01 黄波 创建</remarks>
        public static T Get<T>(CacheKeys.Items key, Func<T> acquire, MethodOptions methodOptions = MethodOptions.Get)
        {
            return BaseGet<T>(key.ToString(), null, DateTime.Now.AddMinutes(CacheExpiry), acquire, methodOptions);
        }

        /// <summary>
        /// 缓存代理
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="key">枚举类型关键字</param>
        /// <param name="acquire">方法变量</param>
        /// <param name="expiry">过期时间</param>
        /// <param name="methodOptions">操作类型</param>
        /// <returns>缓存数据</returns>
        /// <remarks>2013-08-01 黄波 创建</remarks>
        public static T Get<T>(CacheKeys.Items key, DateTime expiry, Func<T> acquire, MethodOptions methodOptions = MethodOptions.Get)
        {
            return BaseGet<T>(key.ToString(), null, expiry, acquire, methodOptions);
        }

        /// <summary>
        /// 缓存代理
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="prefixKey">前缀关键字</param>
        /// <param name="suffix">关键字后接字符串</param>
        /// <param name="acquire">方法变量</param>
        /// <param name="methodOptions">操作类型</param>
        /// <returns>缓存数据</returns>
        /// <remarks>2013-08-01 黄波 创建</remarks>
        public static T Get<T>(CacheKeys.Items prefixKey, string suffix, Func<T> acquire, MethodOptions methodOptions = MethodOptions.Get)
        {
            return BaseGet<T>(prefixKey.ToString(), suffix, DateTime.Now.AddMinutes(CacheExpiry), acquire, methodOptions);
        }

        /// <summary>
        /// 缓存代理
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="prefixKey">前缀关键字</param>
        /// <param name="suffix">关键字后接字符串</param>
        /// <param name="expiry">过期时间</param>
        /// <param name="acquire">方法变量</param>
        /// <param name="methodOptions">操作类型</param>
        /// <returns>缓存数据</returns>
        /// <remarks>2013-08-01 黄波 创建</remarks>
        public static T Get<T>(CacheKeys.Items prefixKey, string suffix, DateTime expiry, Func<T> acquire, MethodOptions methodOptions = MethodOptions.Get)
        {
            return BaseGet<T>(prefixKey.ToString(), suffix, expiry, acquire, methodOptions);
        }

        /// <summary>
        /// 缓存代理
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="prefixKey">前缀关键字</param>
        /// <param name="suffix">关键字后接字符串</param>
        /// <param name="expiry">过期时间</param>
        /// <param name="acquire">方法变量</param>
        /// <param name="methodOptions">操作类型</param>
        /// <returns>缓存数据</returns>
        /// <remarks>2013-08-01 黄波 创建</remarks>
        public static T Get<T>(string prefixKey, string suffix, DateTime expiry, Func<T> acquire, MethodOptions methodOptions = MethodOptions.Get)
        {
            return BaseGet<T>(prefixKey, suffix, expiry, acquire, methodOptions);
        }

        /// <summary>
        /// 缓存代理
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="prefixKey">前缀关键字</param>
        /// <param name="suffix">关键字后接字符串</param>
        /// <param name="expiry">过期时间</param>
        /// <param name="acquire">方法变量</param>
        /// <param name="methodOptions">操作类型</param>
        /// <returns>缓存数据</returns>
        /// <remarks>2013-08-01 黄波 创建</remarks>
        public static T Get<T>(string prefixKey, Func<T> acquire, MethodOptions methodOptions = MethodOptions.Get)
        {
            return BaseGet<T>(prefixKey, "", DateTime.Now.AddMinutes(CacheExpiry), acquire, methodOptions);
        }

        /// <summary>
        /// 缓存代理
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="key">关键字</param>
        /// <param name="suffix">关键字后接字符串(可为空)</param>
        /// <param name="expiry">过期时间(绝对)</param>
        /// <param name="acquire">方法变量</param>
        /// <param name="methodOptions">操作类型</param>
        /// <returns>缓存数据</returns>
        /// <remarks>2013-08-01 黄波 创建</remarks>
        private static T BaseGet<T>(string key, string suffix, DateTime expiry, Func<T> acquire, MethodOptions methodOptions)
        {
            var returnValue = default(T);

            //如果缓存关闭则不执行缓存策略
            if (CacheState != _CacheState.Open.ToString())
            {
                if (acquire == null)
                    return returnValue;
                return acquire();
            }

            //缓存关键字
            var cacheKey = key.ToString();
            if (!string.IsNullOrEmpty(suffix))
            {
                cacheKey += suffix;
            }

            //缓存键是否存在
            var cacheExists = CacheManager.Instance.IsExists(cacheKey);

            //强制重新缓存
            //或者缓存不存在时重新缓存
            if (methodOptions == MethodOptions.ForcedUpdating || !cacheExists)
            {
                //存在则更新 否则设置
                if (acquire != null)
                {
                    returnValue = acquire();
                    if (cacheExists)
                    {
                        CacheManager.Instance.Update(cacheKey, returnValue, expiry);
                    }
                    else
                    {
                        CacheManager.Instance.Set(cacheKey, returnValue, expiry);
                    }
                    return returnValue;
                }
            }
            object o = CacheManager.Instance.Get<object>(cacheKey);
            return CacheManager.Instance.Get<T>(cacheKey);
        }
    }
}

#region 操作类型
/// <summary>
/// 操作类型
/// </summary>
public enum MethodOptions
{
    /// <summary>
    /// 获取
    /// </summary>
    Get,
    /// <summary>
    /// 强制重新缓存
    /// </summary>
    ForcedUpdating,
}
#endregion