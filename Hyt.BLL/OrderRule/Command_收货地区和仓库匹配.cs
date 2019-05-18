using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL;
using Hyt.BLL.Warehouse;
using SoOrderBo = Hyt.BLL.Order.SoOrderBo;

namespace Hyt.BLL.OrderRule
{
    public class Command_收货地区和仓库匹配 : ICommand
    {
        private readonly string rulename = "收货地区和仓库匹配";

        /// <summary>
        /// 返回是否匹配
        /// </summary>
        /// <param name="orderData">订单数据</param>
        /// <returns>true或false</returns>
        /// <remarks>2014-9-10 余勇 创建</remarks>
        public override bool Result(OrderData orderData)
        {
            if (orderData.Order == null || orderData.Order.ReceiveAddressSysNo == 0 || orderData.Order.DefaultWarehouseSysNo == 0 || orderData.ReceiveArea == null || orderData.WarehouseArea == null) return false;
            return orderData.ReceiveArea.ProvinceSysno == orderData.WarehouseArea.ProvinceSysno && orderData.ReceiveArea.CitySysno == orderData.WarehouseArea.CitySysno;
        }

        /// <summary>
        /// 解析命令
        /// </summary>
        /// <param name="command">命令</param>
        /// <returns>命令对象</returns>
        /// <remarks>2014-9-10 余勇 创建</remarks>
        public override ICommand Parse(string command)
        {
            return this.IsContainCommand(rulename, command) ? new Command_收货地区和仓库匹配() : null;
        }
    }
}
