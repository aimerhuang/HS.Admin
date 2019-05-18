using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.Basic;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.OrderRule
{
    /// <summary>
    /// 检查支付状态
    /// </summary>
    /// <remarks>2016-04-01 王江 创建</remarks>
    public class Command_支付状态是 : ICommand
    {
        private readonly string rulename = "支付状态是";

        public int[] keys { get; set; }

        /// <summary>
        /// 到付订单必须完成支付
        /// </summary>
        /// <param name="orderData">订单数据</param>
        /// <returns>true或false</returns>
        /// <remarks>2016-04-01 王江 创建</remarks>
        public override bool Result(OrderData orderData)
        {
            bool flag = false;
            if (orderData != null && orderData.Order != null && keys != null)
            {
                foreach (var item in keys)
                {
                    if (orderData.Order.PayStatus == item)
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
        /// <remarks>2016-04-01 王江 创建</remarks>
        public override ICommand Parse(string command)
        {
            var arg = this.GetArgumentKeys(rulename, command);
            if (arg != null)
            {
                return new Command_支付状态是()
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
