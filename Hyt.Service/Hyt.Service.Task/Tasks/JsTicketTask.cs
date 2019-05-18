using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Hyt.Service.Task.Core;

namespace Hyt.Service.Task.Tasks
{
    /// <summary>
    /// 定时清理任务日志
    /// </summary>
    /// <remarks>2013-10-18 杨浩 添加</remarks>
    [Description("定时清理任务日志")]
    public class ClearTaskLog : ITask
    {
        /// <summary>
        /// 定时清理任务日志
        /// </summary>
        /// <param name="state"></param>
        /// <remarks>2013-10-18 杨浩 添加</remarks>
        public void Execute(object state)
        {
            Hyt.BLL.Sys.SyTaskBo.Instance.ClearTaskLog();
        }
    }
}
