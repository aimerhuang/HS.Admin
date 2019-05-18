using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Transactions;
using Hyt.Service.Task.Core;

namespace Hyt.Service.Task.Tasks
{
    /// <summary>
    /// 清理订单
    /// </summary>
   [Description("定时清理订单")]
    public class OrderTask : ITask
    {
        /// <summary>
        /// 定时清理订单
        /// </summary>
        /// <param name="state"></param>
        /// <remarks>2013-01-21 苟治国 添加</remarks>
        public void Execute(object state)
        {
            using (var tran = new TransactionScope())
            {
                Hyt.BLL.Order.SoOrderBo.Instance.ClearOrder(1);
                tran.Complete();
            }
        }
    }
}
