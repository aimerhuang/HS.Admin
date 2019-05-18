using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.OrderRule
{
    /// <summary>
    /// 朱成果
    /// </summary>
    /// <remarks>2015-1-21 朱成果 创建</remarks>
    public class Command_订单创建时间大于分钟:ICommand
    {
        private readonly string rulename = "订单创建时间大于分钟";

        /// <summary>
        /// 分钟
        /// </summary>
        public int  AddMin { get; set; }

        /// <summary>
        /// 返回结果
        /// </summary>
        /// <param name="orderData"></param>
        /// <returns></returns>
        public override bool Result(OrderData orderData)
        {
           if(orderData!=null&&orderData.Order!=null)
           {
               return orderData.Order.CreateDate.AddMinutes(AddMin) < DateTime.Now;
           }
           return false;
        }

        /// <summary>
        /// 解析命令
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <remarks>2013-11-15 创建 朱成果</remarks>
        public override ICommand Parse(string command)
        {
            var arg = this.GetArgument(rulename, command);
            if (!string.IsNullOrEmpty(arg))
            {
                return new Command_订单创建时间大于分钟()
                {
                    AddMin = Convert.ToInt32(arg)
                };
            }
            else
            {
                return null;
            }
        }
    }
}
