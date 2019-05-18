using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.OrderRule
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>2015/10/10 11:24:56 朱成果 创建</remarks>
    public class Command_订单金额小于 : ICommand
    {
        private readonly string rulename = "订单金额小于";

        public decimal Price { get; set; }

        public override bool Result(OrderData orderData)
        {
            return orderData.Order.OrderAmount <Price;

        }

        public override ICommand Parse(string command)
        {
            var arg = this.GetArgument(rulename, command);
            if (!string.IsNullOrEmpty(arg))
            {
                return new Command_订单金额小于()
                {
                    Price = Convert.ToDecimal(arg)
                };
            }
            else
            {
                return null;
            }
        }
    }
}
