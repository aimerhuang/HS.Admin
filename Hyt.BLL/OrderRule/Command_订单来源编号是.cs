using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.OrderRule
{
    /// <summary>
    /// 检查订单来源编号
    /// </summary>
    /// <remarks>2015-12-23 王江 创建</remarks>
    public class Command_订单来源编号是 : ICommand
    {
        private readonly string rulename = "订单来源编号是";
        /// <summary>
        /// 关键字
        /// </summary>
        public int[] keys { get; set; }
        public override bool Result(OrderData orderData)
        {
            bool flag = false;
            if (orderData != null && orderData.Order != null && keys != null )
            {
                foreach (var item in keys)
                {
                    if (orderData.Order.OrderSource == item)
                    {
                        flag = true;
                        break;
                    }
                }
            }
            return flag;
        }

        /// <summary>
        /// 解析命令
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <remarks>2015-12-23 王江 创建</remarks>
        public override ICommand Parse(string command)
        {
            var arg = this.GetArgumentKeys(rulename, command);
            if (arg != null)
            {
                return new Command_订单来源编号是()
                {
                    keys = arg.Select(m => int.Parse(m)).ToArray()
                };
            }
            else
            {
                return null;
            }
        }
    }
}
