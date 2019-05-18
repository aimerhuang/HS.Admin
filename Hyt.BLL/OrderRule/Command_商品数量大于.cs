using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.OrderRule
{
    public class Command_商品数量大于 : ICommand
    {
        private readonly string rulename = "商品数量大于";
        /// <summary>
        /// 关键字
        /// </summary>
        public int Num { get; set; }
                

        public override bool Result(OrderData orderData)
        {
            if (orderData.OrderItems != null)
            {
                return orderData.OrderItems.Sum(p => p.Quantity) > Num;
            }
            else {
                return false;
            }
        }

        public override ICommand Parse(string command)
        {           
            var arg = this.GetArgument(rulename, command);
            if (!string.IsNullOrEmpty(arg))
            {
                return new Command_商品数量大于()
                {
                    Num = Convert.ToInt32(arg)
                };
            }
            else
            {
                return null;
            }
        }
    }
}
