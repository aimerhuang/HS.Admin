using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Provider
{
    /// <summary>
    /// 管理数据提供器
    /// </summary>
    /// <remarks>
    /// 2013-6-26 杨浩 添加
    /// </remarks>
    public class ProviderManager
    {
        /// <summary>
        /// 设置一项数据提供实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="provider"></param>
        /// <returns></returns>
        /// <remarks>2013-6-26 杨浩 添加</remarks>
        public static void Set<T>(T provider)
        {
            if (ProviderCache<T>.ProviderInstance == null)
                ProviderCache<T>.ProviderInstance = provider;
        }

        /// <summary>
        /// 获取数据提供实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>实例</returns>
        /// <remarks>2013-6-26 杨浩 添加</remarks>
        public static T Get<T>()
        {
            return ProviderCache<T>.ProviderInstance;
        }

        /// <summary>
        /// 添加多项数据提供实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="provider"></param>
        /// <returns></returns>
        /// <remarks>2013-6-26 杨浩 添加</remarks>
        public static void Add<T>(T provider)
        {
            if (!ProviderCache<T>.ProviderInstances.Contains(provider))
            {
                ProviderCache<T>.ProviderInstances.Add(provider);
            }
        }

        /// <summary>
        /// 获取所有数据提供实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>实例</returns>
        /// <remarks>2013-6-26 杨浩 添加</remarks>
        public static List<T> GetManay<T>()
        {
            return ProviderCache<T>.ProviderInstances;
        }

        /// <summary>
        /// 缓存数据提供实例类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <remarks>2013-6-26 杨浩 添加</remarks>
        private class ProviderCache<T>
        {
            public static T ProviderInstance;
            public static List<T> ProviderInstances = new List<T>();
        }
    }
}
