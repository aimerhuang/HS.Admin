using Hyt.DataAccess.Base;
using Hyt.Model;

namespace Hyt.DataAccess.CRM
{
    /// <summary>
    /// 等级积分日志
    /// </summary>
    /// <remarks>2013-07-10 黄波 添加</remarks>
    public abstract class ICrLevelPointLogDao : DaoBase<ICrLevelPointLogDao>
    {
        /// <summary>
        /// 获取等级积分日志详情
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>等级积分日志详情</returns>
        /// <remarks>2013-07-10 黄波 添加</remarks>
        public abstract CrLevelPointLog GetCrLevelPointLogItem(int sysNo);

        /// <summary>
        /// 创建等级积分日志
        /// </summary>
        /// <param name="model">等级积分日志详情</param>
        /// <returns>创建结果</returns>
        /// <remarks>2013-07-10 黄波 添加</remarks>
        public abstract bool InsertCrLevelPointLogItem(CrLevelPointLog model);

        /// <summary>
        /// 查询等级积分日志详情
        /// </summary>
        /// <param name="pager">查询条件</param>
        /// <returns>void</returns>
        /// <remarks>2013-07-10 黄波 添加</remarks>
        public abstract void GetCrLevelPointLogItems(ref Pager<CrLevelPointLog> pager);
    }
}
