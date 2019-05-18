using Hyt.Service.Task.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Hyt.Service.Task.Tasks
{
    /// <summary>
    /// 定时释放未支付订单的锁定库存
    /// </summary>
    [Description("定时释放未支付订单的锁定库存")]
    public class OrderStockTask : ITask
    {
        /// <summary>
        /// 定时撤销未支付订单的锁定库存
        /// </summary>
        /// <param name="state"></param>
        /// <remarks>2017-09-9 罗勤尧 添加</remarks>
        public void Execute(object state)
        {
            using (var tran = new TransactionScope())
            {
                Hyt.BLL.Order.SoOrderBo.Instance.OrderStock(1);
                tran.Complete();
            }
        }
    }
}
