using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Util
{
    /// <summary>
    /// 业务辅助类：暂时找不到合适的命名
    /// </summary>
    public static class YwbUtil
    {
        /// <summary>
        /// 进入业务处理
        /// </summary>
        /// <param name="identity">标识</param>
        /// <param name="seconds">默认为300秒. 300秒自动退出</param>
        /// <returns>
        /// 业务需要判断 如果返回false 则终止程序继续往下执行
        /// 只负责处理不允许二个人“同时调用”某个业务代码，业务需要处理“重复调用”逻辑。
        /// </returns>
        public static bool Enter(string identity)
        {
            return Enter(identity, 300);
        }

        /// <summary>
        /// 进入业务处理
        /// </summary>
        /// <param name="identity">标识</param>
        /// <param name="seconds">默认为300秒. 300秒自动退出</param>
        /// <returns>
        /// 业务需要判断 如果返回false 则终止程序继续往下执行
        /// 只负责处理不允许二个人“同时调用”某个业务代码，业务需要处理“重复调用”逻辑。
        /// </returns>
        public static bool Enter(string identity, int seconds)
        {
            return YwbUtilManager.Instance.Enter(identity, seconds);
        }
        /// <summary>
        /// 业务处理完毕
        /// </summary>
        /// <param name="identity">标识</param>
        public static void Exit(string identity) {
            YwbUtilManager.Instance.Exit(identity);
        }


    }

    internal class YwbUtilManager
    {
        #region singleton
        public static YwbUtilManager Instance
        {
            get
            {
                return Nested.Instance;
            }
        }
        class Nested
        {
            static Nested() { }
            internal static readonly YwbUtilManager Instance = new YwbUtilManager();
        }
        #endregion

        private Dictionary<string, DateTime> _dict = new Dictionary<string, DateTime>();
        private System.Threading.ReaderWriterLockSlim _locker = new System.Threading.ReaderWriterLockSlim();


        public bool Enter(string identity, int seconds)
        {
            _locker.EnterWriteLock();
            var flag = false;
            if (_dict.ContainsKey(identity) == true)
            {
                if (_dict[identity].AddSeconds(seconds) < DateTime.Now)
                {
                    _dict[identity] = DateTime.Now;                    
                    flag = true;
                }
                else
                {
                    flag = false;
                }
            }
            else{
                _dict.Add(identity,DateTime.Now);
                flag = true;
            }
            _locker.ExitWriteLock();
            return flag;
        }

        public void Exit(string identity) {
            _locker.EnterWriteLock();
            if (_dict.ContainsKey(identity)) _dict.Remove(identity);
            _locker.ExitWriteLock();
        }


    }


}
