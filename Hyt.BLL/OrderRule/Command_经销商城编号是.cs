using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.OrderRule
{
    /// <summary>
    /// 检查订单的经销商城编号是否匹配
    /// </summary>
    /// <remarks>2014-9-3 朱成果 创建</remarks>
   public  class Command_经销商城编号是:ICommand
    {
       private readonly string rulename = "经销商城编号是";
        /// <summary>
        /// 关键字
        /// </summary>
        public int[] keys { get; set; }


        /// <summary>
        /// 返回是否匹配
        /// </summary>
        /// <param name="orderData">订单数据</param>
        /// <returns></returns>
        /// <remarks>2014-9-3 朱成果 创建</remarks>
        public override bool Result(OrderData orderData)
        {
            bool flg = false;
            if (orderData != null && orderData.Order!=null && orderData.Order.OrderSource == (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.分销商升舱&&keys!=null)
            {               
                foreach (var k in keys)
                {
                    if(orderData.Order.OrderSourceSysNo==k)
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
                return new Command_经销商城编号是()
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
