using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.DataAccess.Base;

namespace Hyt.DataAccess.Sys
{
    /// <summary>
    /// 锁定任务接口
    /// </summary>
    /// <remarks> 
    /// 2014-06-16 余勇 创建
    /// </remarks>
    public abstract class ISyLockJob : DaoBase<ISyLockJob>
    {
        /// <summary>
        /// 插入锁定任务
        /// </summary>
        /// <param name="model">锁定任务实体</param>
        /// <returns>影响行数</returns>
        /// 2014-06-16 余勇 创建
        public abstract int Insert(SyLockJob model);

        /// <summary>
        /// 通过任务池编号获取锁定任务信息
        /// </summary>
        /// <param name="jobPoolSysNo">任务池编号</param>
        /// <returns>返回锁定任务实体</returns>
        /// 2014-06-16 余勇 创建
        public abstract SyLockJob Get(int jobPoolSysNo);

        /// <summary>
        /// 通过任务池编号删除锁定任务信息
        /// </summary>
        /// <param name="jobPoolSysNo">任务池编号</param>
        /// <returns>影响行数</returns>
        /// 2014-06-16 余勇 创建
        public abstract int Delete(int jobPoolSysNo);

        /// <summary>
        /// 获取大于解锁日期的自动解锁任务
        /// </summary>
        /// <returns>自动解锁任务列表</returns>
        /// 2014-06-16 余勇 创建
        public abstract List<int> GetOverTimeLockJobs();
    }
}