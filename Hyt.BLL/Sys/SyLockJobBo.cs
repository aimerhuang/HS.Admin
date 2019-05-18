using Hyt.BLL.Log;
using Hyt.DataAccess.Sys;
using Hyt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Sys
{
    /// <summary>
    /// 任务池分配
    /// </summary>
    /// <remarks>2013-06-21 余勇 创建</remarks>
    public class SyLockJobBo : BOBase<SyLockJobBo>
    {

        /// <summary>
        /// 插入锁定任务
        /// </summary>
        /// <param name="model">锁定任务实体</param>
        /// <returns>影响行数</returns>
        /// 2014-06-16 余勇 创建
        public int Insert(SyLockJob model)
        {
           return ISyLockJob.Instance.Insert(model);
        }

        /// <summary>
        /// 通过任务池编号获取锁定任务信息
        /// </summary>
        /// <param name="jobPoolSysNo">任务池编号</param>
        /// <returns>返回锁定任务实体</returns>
        /// 2014-06-16 余勇 创建
        public  SyLockJob Get(int jobPoolSysNo)
        {
            return ISyLockJob.Instance.Get(jobPoolSysNo);
        }

        /// <summary>
        /// 通过任务池编号删除锁定任务信息
        /// </summary>
        /// <param name="jobPoolSysNo">任务池编号</param>
        /// <returns>影响行数</returns>
        /// 2014-06-16 余勇 创建
        public int Delete(int jobPoolSysNo)
        {
            return ISyLockJob.Instance.Delete(jobPoolSysNo);
        }

        /// <summary>
        /// 获取大于解锁日期的自动解锁任务
        /// </summary>
        /// <returns>自动解锁任务列表</returns>
        /// 2014-06-16 余勇 创建
        public List<int> GetOverTimeLockJobs()
        {
            return ISyLockJob.Instance.GetOverTimeLockJobs();
        }
    }
}
