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
    /// 定时执行订单返利操作
    /// </summary>
    /// <remarks>2016-1-13 杨浩 创建</remarks>
    [Description("定时执行订单返利操作")]
    public class RebatesRecordTask:ITask
    {
        private static object lockHelper = new object();
        public void Execute(object state)
        {
            lock (lockHelper)
            {
                try
                {
                    Hyt.BLL.SellBusiness.CrCustomerRebatesRecordBo.Instance.CrCustomerRebatesRecordToCustomer(14);
                }
                catch (Exception ex)
                {
                    SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "定时分配任务池错误:" + ex.Message,
                                 LogStatus.系统日志目标类型.用户, 0, ex);
                }
            }                       
        }
    }
}
