using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model.SystemPredefined;

namespace Hyt.BLL.OrderRule
{
    /// <summary>
    /// 第三方快递
    /// </summary>
    /// <remarks>2014-9-10  余勇 创建</remarks>
    public class Command_第三方快递 : ICommand
    {
        private readonly string rulename = "第三方快递";

        /// <summary>
        /// 返回是否匹配
        /// </summary>
        /// <param name="orderData">订单数据</param>
        /// <returns>true或false</returns>
        /// <remarks>2014-9-10 余勇 创建</remarks>
        public override bool Result(OrderData orderData)
        {
            if (orderData.Order == null || orderData.Order.DeliveryTypeSysNo == 0) return false;
            return orderData.Order.DeliveryTypeSysNo == (int)DeliveryType.第三方快递;
        }

        /// <summary>
        /// 解析命令
        /// </summary>
        /// <param name="command">命令</param>
        /// <returns>命令对象</returns>
        /// <remarks>2014-9-10 余勇 创建</remarks>
        public override ICommand Parse(string command)
        {
            return this.IsContainCommand(rulename, command) ? new Command_第三方快递() : null;
        }
    }
}
