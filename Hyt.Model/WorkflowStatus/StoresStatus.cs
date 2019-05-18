using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.WorkflowStatus
{
    public class StoresStatus
    {
        public enum 保证金订单状态
        {
            待审核 = 10,
            已审核 = 20,
            已完成 = 30,
            作废 = -10
        }

        public enum 保证金订单支付状态
        {
            待支付 = 10,
            已支付 = 20,
            支付异常 = 30
        }
    }
}
