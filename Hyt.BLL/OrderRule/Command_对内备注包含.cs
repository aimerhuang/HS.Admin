using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.OrderRule
{
    /// <summary>
    /// 对内备注包含
    /// </summary>
    /// <remarks>2014-9-3 朱成果 创建</remarks>
    public class Command_对内备注包含:ICommand
    {
        private readonly string rulename = "对内备注包含";

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
        public override  bool Result(OrderData orderData)
        {

            bool flg = false;
            if (orderData != null && orderData.Order != null && keys != null && !string.IsNullOrEmpty(orderData.Order.InternalRemarks))  //添加条件InternalRemarks不能为空 余勇 2014-09-25
            {
                foreach (var k in keys)
                {
                    if (orderData.Order.InternalRemarks.Contains(k))
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
        /// <param name="command">命令</param>
        /// <returns></returns>
        /// <remarks>2014-9-3 朱成果 创建</remarks>
        public override ICommand Parse(string command)
        {
            var arg = this.GetArgumentKeys(rulename, command);
            if (arg != null)
            {
                return new Command_对内备注包含()
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
