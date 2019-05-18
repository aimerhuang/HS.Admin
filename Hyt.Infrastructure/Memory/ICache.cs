using System.Collections.Generic;

namespace Hyt.Infrastructure.Memory
{
    /// <summary>
    /// 缓存接口
    /// </summary>
    /// <remarks>2013-8-2 杨浩 添加</remarks>
    public interface ICache
    {
        /// <summary>
        /// 是否存在缓存项
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        bool Exists(string key);
        /// <summary>
        ///根据键获取值
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">键</param>
        /// <returns></returns>
        T Get<T>(string key);
        /// <summary>
        ///根据键获取值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        object Get(string key);

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="cacheTime">单位/分</param>
        /// <param name="cachePolicy">缓存策略</param>
        void Set(string key, object value, int cacheTime, CachePolicy cachePolicy = CachePolicy.Absolute);
        /// <summary>
        /// 移除缓存项
        /// </summary>
        /// <param name="key">key</param>
        void Remove(string key);
        /// <summary>
        /// 根据匹配符移除缓存项
        /// </summary>
        /// <param name="pattern">pattern</param>
        void RemoveByPattern(string pattern);
        /// <summary>
        ///清楚所有缓存
        /// </summary>
        void Clear();

        /// <summary>
        ///获取所有key
        /// </summary>
        IList<string> GetAllKey();

    }

    /// <summary>
    /// 缓存策略
    /// </summary>
    public enum CachePolicy
    {
        /// <summary>
        /// 相对
        /// </summary>
        Relative,
        /// <summary>
        /// 绝对
        /// </summary>
        Absolute
    }
}
