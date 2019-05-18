using Hyt.DataAccess.Base;
using Hyt.Model;

namespace Hyt.DataAccess.CRM
{
    /// <summary>
    /// 等级变更日志
    /// </summary>
    /// <remarks>2013-07-10 黄波 添加</remarks>
    public abstract class ICrLevelLogDao : DaoBase<ICrLevelLogDao>
    {
        /// <summary>
        /// 获取等级变更日志详情
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>等级变更日志详情</returns>
        /// <remarks>2013-07-10 黄波 添加</remarks>
        public abstract CrLevelLog GetCrLevelLogItem(int sysNo);

        /// <summary>
        /// 插入等级变更日志
        /// </summary>
        /// <param name="model">等级变更日志详情</param>
        /// <returns>创建结果</returns>
        /// <remarks>2013-07-10 黄波 添加</remarks>
        public abstract bool InsertCrLevelLogItem(CrLevelLog model);

        /// <summary>
        /// 查询等级变更日志详情
        /// </summary>
        /// <param name="pager">查询条件</param>
        /// <returns>void</returns>
        /// <remarks>2013-07-10 黄波 添加</remarks>
        public abstract void GetCrLevelLogItems(ref Pager<CrLevelLog> pager);
    }
}
