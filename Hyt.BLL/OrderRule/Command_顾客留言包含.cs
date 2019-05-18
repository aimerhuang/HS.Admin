using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.OrderRule
{
    /// <summary>
    /// 如果留言中有包含"、" 未处理转义字符
    /// </summary>
    public class Command_顾客留言包含 : ICommand
    {
        private readonly  string rulename = "顾客留言包含";
        /// <summary>
        /// 关键字
        /// </summary>
        public string[] keys { get; set; }


        public override bool Result(OrderData orderData)
        {
            if (keys.Length < 1) return false;
            if (string.IsNullOrEmpty(orderData.Order.CustomerMessage)) return false;

            foreach (var k in keys) {
                if (orderData.Order.CustomerMessage.Contains(k) == true) {
                    return true;
                }
            }
            return false;            
        }

        public override ICommand Parse(string command)
        {
            var arg = this.GetArgumentKeys(rulename, command);
            if (arg != null)
            {
                return new Command_顾客留言包含()
                {
                    keys = arg
                };
            }
            else
            {
                return null;
            }
        }
    }
}
