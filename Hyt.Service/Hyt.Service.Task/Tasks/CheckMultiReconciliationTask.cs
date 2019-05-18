using Hyt.Service.Task.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Service.Task.Tasks
{
    /// <summary>
    /// 加盟商定时对账，确认收款单
    /// </summary>
    [Description("加盟商定时对账，确认收款单")]
    public  class CheckMultiReconciliationTask : ITask
    {
        private static bool _starting;
        /// <summary>
        /// 执行任务的方法
        /// </summary>
        /// <param name="state">状态</param>
        /// <returns>空</returns>
        /// <remarks>2014-08-22 朱成果 创建</remarks>
        public void Execute(object state)
        {
            if (!_starting)
            {
                _starting = true;
                try
                {
                    Hyt.BLL.Finance.FinanceBo.Instance.CheckMultiReconciliation(1, 1000);
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
