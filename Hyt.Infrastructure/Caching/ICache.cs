using System;
using System.Collections.Generic;

/// <summary>
/// 缓存策略接口
/// </summary>
namespace Hyt.Infrastructure.Caching
{
    /// <summary>
    /// 缓存接口
    /// </summary>
    /// 杨浩
    /// <remarks>2013-08-01 黄波 重构</remarks>
    public interface ICache
    {
        /// <summary>
        /// 设置数据缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        void Set(string key, object value);       
        /// <summary>
        /// 设置数据缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expiry">过期时间</param>
        void Set(string key, object value, DateTime expiry);

        /// <summary>
        /// 读取数据缓存
        /// </summary>
        /// <param name="key">键</param>
        T Get<T>(string key);

        /// <summary>
        /// 替换更新数据缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        void Update(string key, object value);

        /// <summary>
        /// 替换更新数据缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expiry">过期时间</param>
        void Update(string key, object value, DateTime expiry);

        /// <summary>
        ///根据Key删除指定缓存
        /// </summary>
        /// <param name="key">键</param>
        bool Delete(string key);

        /// <summary>
        ///根据Key前缀删除缓存
        /// </summary>
        /// <param name="prefix">键前缀</param>
        bool DeleteByPrefix(string prefix);

        /// <summary>
        /// 移除全部缓存
        /// </summary>
        void RemovAllCache();

        /// <summary>
        /// 获取所有的缓存key
        /// </summary>
        /// <returns></returns>
        List<string> GetAllKeys();

        /// <summary>
        /// 是否存在指定的缓存key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool IsExists(string key);
    }
}