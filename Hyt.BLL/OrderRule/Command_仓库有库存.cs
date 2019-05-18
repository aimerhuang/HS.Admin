using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.Order;
using Hyt.Util;

namespace Hyt.BLL.OrderRule
{
    /// <summary>
    /// 仓库有库存
    /// </summary>
    /// <remarks>2014-9-25  余勇 创建</remarks>
    public class Command_仓库有库存 : ICommand
    {
        private readonly string rulename = "仓库有库存";

        /// <summary>
        /// 返回是否匹配
        /// </summary>
        /// <param name="orderData">订单数据</param>
        /// <returns>true或false</returns>
        /// <remarks>2014-9-10 余勇 创建</remarks>
        public override bool Result(OrderData orderData)
        {
            if (orderData.Order != null && orderData.OrderItems != null && orderData.OrderItems.Any() && orderData.Order.DefaultWarehouseSysNo>0)
            {
                var dic = new Dictionary<string, int>();
                orderData.OrderItems.ForEach(x => dic.Add(Hyt.BLL.Product.PdProductBo.Instance.GetProductErpCode(x.ProductSysNo), x.Quantity));
                var list = SoOrderBo.Instance.GetInventory(dic.Select(x => x.Key).ToArray(), orderData.Order.DefaultWarehouseSysNo.ToString());
                return list != null && list.Any() && list.TrueForAll(x => x.Quantity > 0 && dic[x.MaterialNumber] <= x.Quantity);
            }
            return false;

        }

        /// <summary>
        /// 解析命令
        /// </summary>
        /// <param name="command">命令</param>
        /// <returns>命令对象</returns>
        /// <remarks>2014-9-10 余勇 创建</remarks>
        public override ICommand Parse(string command)
        {
            return this.IsContainCommand(rulename, command) ? new Command_仓库有库存() : null;
        }
    }
}
