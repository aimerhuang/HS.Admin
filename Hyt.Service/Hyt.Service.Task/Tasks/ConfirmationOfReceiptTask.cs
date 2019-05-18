using Hyt.BLL.Log;
using Hyt.Model.WorkflowStatus;
using Hyt.Service.Task.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Service.Task.Tasks
{
    /// <summary>
    /// 定时执行确认收货操作
    /// </summary>
    /// <remarks>2016-1-13 杨浩 创建</remarks>
    [Description("定时执行确认收货操作")]
    public class ConfirmationOfReceiptTask : ITask
    {        
        public void Execute(object state)
        {           
                try
                {
                    Hyt.BLL.Order.SoOrderBo.Instance.AutoConfirmationOfReceipt(7);
                }
                catch (Exception ex)
                {
                    SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "定时分配任务池错误:" + ex.Message,
                                 LogStatus.系统日志目标类型.用户, 0, ex);
                }
                                  
        }
    }
}
