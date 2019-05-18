using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Sys;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Sys
{

    /// <summary>
    /// 自动分配系统任务
    /// </summary>
    /// <remarks> 
    /// 2013-06-21 余勇 创建
    /// </remarks>
    public class SyLockJobDaoImpl : ISyLockJob
    {

        /// <summary>
        /// 插入锁定任务
        /// </summary>
        /// <param name="model">锁定任务实体</param>
        /// <returns>影响行数</returns>
        /// 2014-06-16 余勇 创建
        public override int Insert(SyLockJob model)
        {
            return Context.Insert("SyLockJob", model)
                .AutoMap()
                .Execute();
        }

        /// <summary>
        /// 通过任务池编号获取锁定任务信息
        /// </summary>
        /// <param name="jobPoolSysNo">任务池编号</param>
        /// <returns>返回锁定任务实体</returns>
        /// 2014-06-16 余勇 创建
        public override SyLockJob Get(int jobPoolSysNo)
        {
            return Context.Sql(@"SELECT * FROM SyLockJob WHERE JobPoolSysNo=@jobPoolSysNo")
               .Parameter("jobPoolSysNo", jobPoolSysNo)
               .QuerySingle<SyLockJob>();
        }

        /// <summary>
        /// 通过任务池编号删除锁定任务信息
        /// </summary>
        /// <param name="jobPoolSysNo">任务池编号</param>
        /// <returns>影响行数</returns>
        /// 2014-06-16 余勇 创建
        public override int Delete(int jobPoolSysNo)
        {
            return Context.Sql(@"DELETE SyLockJob WHERE JobPoolSysNo=@jobPoolSysNo")
               .Parameter("jobPoolSysNo", jobPoolSysNo)
               .Execute();
        }

        /// <summary>
        /// 获取大于解锁日期的自动解锁任务
        /// </summary>
        /// <returns>自动解锁任务列表</returns>
        /// 2014-06-16 余勇 创建
        public override List<int> GetOverTimeLockJobs()
        {
            return Context.Sql(@"select jobpoolsysno from SyLockJob where unlockstate=@1 and unlockdate<@1", (int)SystemStatus.锁定任务状态.自动解锁, DateTime.Now)
                          .QueryMany<int>();
        }
    }
}
