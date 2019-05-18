using Hyt.Model.SystemPredefined;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.OrderRule
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>2015/11/5 8:43:21 朱成果 创建</remarks>
    public class Command_当日达配送商品 : ICommand
    {
        private readonly string rulename = "当日达配送商品";
        /// <summary>
        /// 关键字
        /// </summary>
        public string[] keys { get; set; }

        /// <summary>
        /// 返回是否匹配
        /// </summary>
        /// <param name="orderData">订单数据</param>
        /// <returns></returns>
        /// <remarks>2014-9-3 朱成果 创建</remarks>
        public override bool Result(OrderData orderData)
        {
            bool flg = false;
            if (orderData.Order == null || orderData.Order.DeliveryTypeSysNo == 0) return false;
            flg = orderData.Order.DeliveryTypeSysNo == (int)DeliveryType.百城当日达 ||
                 orderData.Order.DeliveryTypeSysNo == (int)DeliveryType.定时百城当日达 ||
                 orderData.Order.DeliveryTypeSysNo == (int)DeliveryType.加急百城当日达 ||
                 orderData.Order.DeliveryTypeSysNo == (int)DeliveryType.普通百城当日达;
            if (flg && orderData != null && orderData.OrderItems != null && keys != null)
            {
                foreach (var item in orderData.OrderItems)
                {
                    var erpcode = Hyt.BLL.Product.PdProductBo.Instance.GetProductErpCode(item.ProductSysNo);//产品编号
                    flg = (erpcode != null) && keys.Contains(erpcode);
                    if (flg)
                    {
                        break;
                    }
                }
            }
            return flg;
        }

        /// <summary>
        /// 解析命令
        /// </summary>
        /// <param name="command">命令</param>
        /// <returns></returns>
        /// <remarks>2014-9-3 朱成果 创建</remarks>
        public override ICommand Parse(string command)
        {
            var arg = this.GetArgumentKeys(rulename, command);
            if (arg != null)
            {
                return new Command_当日达配送商品()
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
