using Hyt.DataAccess.Base;
using Hyt.Model;

namespace Hyt.DataAccess.CRM
{
    /// <summary>
    /// 惠源币日志
    /// </summary>
    /// <remarks>2013-07-10 黄波 添加</remarks>
    public abstract class ICrExperienceCoinLogDao : DaoBase<ICrExperienceCoinLogDao>
    {
        /// <summary>
        /// 获取惠源币日志详情
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>惠源币日志详情详情</returns>
        /// <remarks>2013-07-10 黄波 添加</remarks>
        public abstract CrExperienceCoinLog GetCrExperienceCoinLogItem(int sysNo);

        /// <summary>
        /// 插入惠源币日志
        /// </summary>
        /// <param name="model">惠源币日志详情</param>
        /// <returns>创建结果</returns>
        /// <remarks>2013-07-10 黄波 添加</remarks>
        public abstract bool InsertCrExperienceCoinLogItem(CrExperienceCoinLog model);

        /// <summary>
        /// 查询惠源币日志详情
        /// </summary>
        /// <param name="pager">查询条件</param>
        /// <returns>void</returns>
        /// <remarks>2013-07-10 黄波 添加</remarks>
        public abstract void GetCrExperienceCoinLogItems(ref Pager<CrExperienceCoinLog> pager);

        /// <summary>
        /// 获取日志中用户的惠源币余额
        /// </summary>
        /// <param name="customerSysNo">会员编号</param>
        /// <returns>惠源币余额</returns>
        /// <remarks>2013-07-10 黄波 添加</remarks>
        public abstract int GetCrExperienceCoinSurplus(int customerSysNo);
    }
}
