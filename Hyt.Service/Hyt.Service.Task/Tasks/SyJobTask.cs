using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Hyt.BLL.Log;
using Hyt.Model.WorkflowStatus;
using Hyt.Service.Task.Core;

namespace Hyt.Service.Task.Tasks
{
    /// <summary>
    /// 定时分配任务池
    /// </summary>
    /// <remarks>2013-11-8 余勇 创建</remarks>
    [Description("定时分配任务池")]
    public class SyJobTask : ITask
    {
        private static bool _starting;

        /// <summary>
        /// 执行任务的方法
        /// </summary>
        /// <param name="state">状态</param>
        /// <returns>空</returns>
        /// <remarks>2013-09-28 余勇 创建</remarks>
        public void Execute(object state)
        {
            if (!_starting)
            {
                _starting = true;
                try
                {
                    Hyt.BLL.Sys.SyJobPoolManageBo.Instance.AutoAssignJob();
                }
                catch (Exception ex)
                {
                    SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "定时分配任务池错误:" + ex.Message,
                                 LogStatus.系统日志目标类型.用户, 0, ex);
                }
                _starting = false;
            }
        }
    }
}
