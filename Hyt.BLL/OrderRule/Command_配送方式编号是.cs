using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.OrderRule
{
    /// <summary>
    /// 配送方式编号是
    /// </summary>
    /// <remarks>2015-01-21 朱成果 创建</remarks>
    public class Command_配送方式编号是:ICommand
    {
        private readonly string rulename = "配送方式编号是";
        /// <summary>
        /// 配送方式编号
        /// </summary>
        public  int[] keys { get; set; }

        /// <summary>
        /// 返回是否匹配
        /// </summary>
        /// <param name="orderData">订单数据</param>
        /// <returns>true或false</returns>
        /// <remarks>2015-01-21 朱成果 创建</remarks>
        public override bool Result(OrderData orderData)
        {
            bool flg = false;
            if (orderData != null && orderData.Order != null  && keys != null)
            {
                foreach (var k in keys)
                {
                    if (orderData.Order.DeliveryTypeSysNo == k)
                    {
                        flg = true;
                        break;
                    }
                }
            }
            return flg;
        }

        /// <summary>
        /// 赋值
        /// </summary>
        /// <param name="command">命令</param>
        /// <returns></returns>
        /// <remarks>2015-01-21 朱成果 创建</remarks>
        public override ICommand Parse(string command)
        {
             var arg = this.GetArgumentKeys(rulename, command);
            if (arg != null)
            {
                return new Command_配送方式编号是()
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
