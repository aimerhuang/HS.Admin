using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Hyt.Service.Task.Core;
using System.Threading;
using Hyt.BLL.Log;
using Hyt.Model.WorkflowStatus;

namespace Hyt.Service.Task.Tasks
{
    /// <summary>
    /// 分销商城配送单报表定时生成Excel
    /// </summary>
    [Description("分销商城配送单报表定时生成")]
    public class DistributionReportTask:ITask
    {
        private static bool _starting;
        /// <summary>
        /// 执行任务的方法
        /// </summary>
        /// <param name="state">状态</param>
        /// <returns>空</returns>
        /// <remarks>2013-02-20 朱成果 创建</remarks>
        public void Execute(object state)
        {
            if (!_starting)
            {
                _starting = true;
                try
                {
                    Hyt.BLL.Report.ReportBO.Instance.ExportPickingReportExcle(DateTime.Now.AddMonths(-1).ToString("yyyyMM"));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    _starting = false;
                }
            }
            
        }
    }
}
