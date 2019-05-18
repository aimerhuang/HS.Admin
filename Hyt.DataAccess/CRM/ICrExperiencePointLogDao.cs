using Hyt.DataAccess.Base;
using Hyt.Model;

namespace Hyt.DataAccess.CRM
{
    /// <summary>
    /// 经验积分
    /// </summary>
    /// <remarks>2013-07-10 黄波 添加</remarks>
    public abstract class ICrExperiencePointLogDao : DaoBase<ICrExperiencePointLogDao>
    {
        /// <summary>
        /// 获取经验积分日志详情
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>经验积分日志详情</returns>
        /// <remarks>2013-07-10 黄波 添加</remarks>
        public abstract CrExperiencePointLog GetCrExperiencePointLogItem(int sysNo);

        /// <summary>
        /// 插入经验积分日志
        /// </summary>
        /// <param name="model">经验积分信息</param>
        /// <returns>创建结果</returns>
        /// <remarks>2013-07-10 黄波 添加</remarks>
        public abstract bool InsertCrExperiencePointLogItem(CrExperiencePointLog model);

        /// <summary>
        /// 查询经验积分日志
        /// </summary>
        /// <param name="pager">查询条件</param>
        /// <returns>void</returns>
        /// <remarks>2013-07-10 黄波 添加</remarks>
        public abstract void GetCrExperiencePointLogItems(ref Pager<CrExperiencePointLog> pager);

        /// <summary>
        /// 获取日志中用户的经验积分余额
        /// </summary>
        /// <param name="customerSysNo">会员编号</param>
        /// <returns>经验积分余额</returns>
        /// <remarks>2013-07-10 黄波 添加</remarks>
        public abstract int GetCrExperiencePointSurplus(int customerSysNo);
    }
}
