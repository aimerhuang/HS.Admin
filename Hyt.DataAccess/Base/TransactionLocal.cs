using System;
using System.Configuration;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace Hyt.DataAccess.Base
{
    /// <summary>
    /// 为当前线程绑定一个事务
    /// </summary>
    /// <remarks> 2014-05-22 杨浩 创建 </remarks>
    public class TransactionLocal : IDisposable
    {
        /// <summary>
        /// Oracle连接字符串
        /// </summary>
        private static readonly string OracleConnectionString = "OracleConnectionString";

        private readonly ThreadLocal<IDbContext> _threadLocal = new ThreadLocal<IDbContext>(() => new DbContext().ConnectionStringName(OracleConnectionString));

        /// <summary>
        /// 初始构造函数
        /// </summary>
        /// <param name="level">事务隔离级别</param>
        public TransactionLocal(System.Data.IsolationLevel level = System.Data.IsolationLevel.Serializable)
        {
            Initialize(level);
        }

        /// <summary>
        /// 获取当前托管线程的唯一标识符
        /// </summary>
        public static int GetCurrentThreadId
        {
            get { return Thread.CurrentThread.ManagedThreadId; }
        }

        /// <summary>
        /// 获取当前线程的事务连接
        /// </summary>
        /// <returns></returns>
        public IDbContext Context
        {
            get
            {
                return _threadLocal.Value;
            }
        }

        /// <summary>
        /// 初始化事务
        /// </summary>
        /// <param name="level"></param>
        private void Initialize(System.Data.IsolationLevel level)
        {
            _threadLocal.Value.UseTransaction(true);
            _threadLocal.Value.Data.IsolationLevel = (Hyt.DataAccess.Base.IsolationLevel)level;
        }

        /// <summary>
        /// 开始事务
        /// </summary>
        /// <param name="level"></param>
        private void Begin(System.Data.IsolationLevel level)
        {
            Initialize(level);
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void Submit()
        {
            _threadLocal.Value.Commit();
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void Rollback()
        {
            _threadLocal.Value.Rollback();
            _threadLocal.Value.Dispose();
        }

        /// <summary>
        /// 资源释放
        /// </summary>
        public void Dispose()
        {
            this._threadLocal.Value.Dispose();
            this._threadLocal.Dispose();
        }
    }
}
