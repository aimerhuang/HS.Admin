using Extra.PushService;
using Hyt.Service.Task.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Service.Task.Tasks
{
    /// <summary>
    /// 分销商城配送单报表定时生成Excel
    /// </summary>
    [Description("服务端推送服务关闭的情况下定时器执行启动，时刻检测")]
    public class PushServiceCloseTask : ITask
    {
        public void Execute(object state)
        {
            AgentPushServiceSingleton.Instance.CheckSuperWebSocketStart();
        }
    }
}
