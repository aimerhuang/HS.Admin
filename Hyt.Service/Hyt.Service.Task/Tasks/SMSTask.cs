using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Hyt.Service.Task.Core;
using System.Threading;

namespace Hyt.Service.Task.Tasks
{
    /// <summary>
    /// 短信发送任务
    /// </summary>
    [Description("短信发送任务")]
    public class SMSTask : ITask
    {
        /// <summary>
        /// 短信发送任务
        /// </summary>
        /// <param name="state"></param>
        /// <remarks>2013-01-21 苟治国 添加</remarks>
        public void Execute(object state)
        {
            Hyt.BLL.Sys.SyTaskBo.Instance.SmsTask(20);
        }
    }
}
