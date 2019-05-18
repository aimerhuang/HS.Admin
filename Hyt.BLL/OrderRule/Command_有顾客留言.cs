using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.OrderRule
{
    /// <summary>
    /// 有顾客留言不自动审单
    /// </summary>
    /// <remarks>2015-10-20 王江 创建</remarks>
    public class Command_有顾客留言 : ICommand
    {
        private readonly string rulename = "有顾客留言";
        public override bool Result(OrderData orderData)
        {
            bool flag = !string.IsNullOrEmpty(orderData.Order.CustomerMessage);
            return flag;
        }

        public override ICommand Parse(string command)
        {
            return this.IsContainCommand(rulename, command) ? new Command_有顾客留言() : null;
        }
    }
}
