using Hyt.Model;
using Hyt.Model.Parameter;

namespace Hyt.DataAccess.Log
{
    /// <summary>
    /// 系统日志数据访问 抽象类 
    /// </summary>
    /// <remarks>2013-06-27 吴文强 创建</remarks>
    public abstract class ISySystemLogDao : Hyt.DataAccess.Base.DaoBase<ISySystemLogDao>
    {
        /// <summary>
        /// create
        /// </summary>
        /// <param name="log">model</param>
        /// <returns></returns>
        /// <remarks>2013-08-14 朱家宏 创建</remarks>
        public abstract void Create(SySystemLog log);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2013-08-14 朱家宏 创建</remarks>
        public abstract Pager<SySystemLog> Query(ParaSystemLogFilter filter);

        /// <summary>
        /// 获取日志信息
        /// </summary>
        /// <param name="source">系统日志来源</param>
        /// <param name="targetType">系统日志目标类型</param>
        /// <param name="targetSysNo">来源系统编号</param>
        /// <returns>系统日志列表</returns>
        /// <remarks>2013-09-09 沈强 创建</remarks>
        public abstract System.Collections.Generic.IList<SySystemLog> Get(Model.WorkflowStatus.LogStatus.系统日志来源 source,
                                      Model.WorkflowStatus.LogStatus.系统日志目标类型 targetType, int targetSysNo);
    }
}
