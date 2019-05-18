using Hyt.BLL.Distribution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.OrderRule
{
    /// <summary>
    /// 经销商商城名称是
    /// </summary>
    /// <remarks>2014-9-3 朱成果 创建</remarks>
    public  class Command_经销商城名称是 : ICommand
    {
        private readonly string rulename = "经销商城名称是";
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
           bool flg=false;
           if (orderData != null && orderData.Order != null && orderData.Order.OrderSource == (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.分销商升舱 && keys != null && !string.IsNullOrEmpty(orderData.ShopName))
           {
                foreach (var k in keys)
                {
                    if (orderData.ShopName.Equals(k))
                    {
                        flg = true;
                        break;
                    }
                }
           }
           return flg;
        }

        /// <summary>
        /// 解析命令
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <remarks>2014-9-3 朱成果 创建</remarks>
       public override ICommand Parse(string command)
       {
           var arg = this.GetArgumentKeys(rulename, command);
           if (arg != null)
           {
               return new Command_经销商城名称是()
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
