using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Hyt.Service.Task.Core;

namespace Hyt.Service.Task.Tasks
{
    /// <summary>
    /// 调整客户等级
    /// </summary>
    /// <remarks>2013-01-21 苟治国 添加</remarks>
    [Description("定时调整客户等级")]
    public class GradeTask : ITask
    {
        /// <summary>
        /// 调整客户等级
        /// </summary>
        /// <param name="state"></param>
        /// <remarks>2013-01-21 苟治国 添加</remarks>
        public void Execute(object state)
        {
            Hyt.BLL.Sys.SyTaskBo.Instance.AdjustmentGradeTask(12);
        }
    }
}
