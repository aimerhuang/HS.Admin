using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Infrastructure
{
    /// <summary>
    /// 线程安全类
    /// </summary>
    /// <remarks>2014-08-19 杨浩 添加</remarks>
    public static class ThreadSafe
    {
        private readonly static Dictionary<string, bool> _syncList = new Dictionary<string, bool>();
        private static readonly object _lockThis = new object();
        private const string _msg = "操作进行中,请稍后再试...";
        

        /// <summary>
        /// 操作加锁保证线程安全
        /// </summary>
        /// <param name="syncId">线程同步Id</param>
        /// <param name="syncFunc">线程同步方法</param>
        /// <returns></returns>
        /// <remarks>2014-08-19 杨浩 添加</remarks>
        public static SyncResult Lock(string syncId, Action syncFunc)
        {
            lock (_lockThis)
            {
                if (_syncList.ContainsKey(syncId))
                    return new SyncResult
                    {
                        IsError=true,
                        Message = _msg
                    };
                _syncList.Add(syncId, true);
            }
            //执行线程安全方法
            syncFunc();
            _syncList.Remove(syncId);
            return new SyncResult
            {
                IsError = false
            };
        }

        /// <summary>
        /// 操作加锁保证线程安全
        /// </summary>
        /// <param name="syncId">线程同步Id</param>
        /// <param name="syncFunc">线程同步方法</param>
        /// <returns></returns>
        /// <remarks>2014-08-19 杨浩 添加</remarks>
        public static SyncResult<T> Lock<T>(string syncId, Func<T> syncFunc)
        {
            var data = default(T);
            lock (_lockThis)
            {
                if (_syncList.ContainsKey(syncId))
                    return new SyncResult<T>
                    {
                        IsError=true,
                        Message = _msg
                    };
                _syncList.Add(syncId, true);
            }
            //执行线程安全方法
           data= syncFunc();
           _syncList.Remove(syncId);
            return new SyncResult<T>
            {
                IsError = false,
                Data = data,
            };
        }
    }

    /// <summary>
    /// 线程同步结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SyncResult<T>
    {
        /// <summary>
        /// 是否有错
        /// </summary>
        public bool IsError { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 方法的返回数据
        /// </summary>
        public T Data { get; set; }
    }

    public class SyncResult
    {
        /// <summary>
        /// 是否有错
        /// </summary>
        public bool IsError { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
    }
}
