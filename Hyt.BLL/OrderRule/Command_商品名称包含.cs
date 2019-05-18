using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.OrderRule
{
    public class Command_商品名称包含:ICommand
    {
        private readonly string rulename = "商品名称包含";

        /// <summary>
        /// 关键字
        /// </summary>
        public string[] keys { get; set; }

        /// <summary>
        /// 返回是否匹配
        /// </summary>
        /// <param name="orderData">订单数据</param>
        /// <returns>商品名称包含返回true,否则返回false</returns>
        /// <remarks>2015-9-15 谭显锋 创建</remarks>
        public override bool Result(OrderData orderData)
        {

            bool flg = false;
            if (orderData != null && orderData.OrderItems != null && keys != null)  //
            {
                foreach (var k in keys)
                {
                    var productList = orderData.OrderItems.Select(s => s.ProductName).ToList();
                    foreach (var p in productList)
                    {
                        if (p.Contains(k))
                        {
                            flg = true;
                            break;
                        }
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
        /// <remarks>2015-9-14 谭显锋 创建</remarks>
        public override ICommand Parse(string command)
        {
            var arg = this.GetArgumentKeys(rulename, command);
            if (arg != null)
            {
                return new Command_商品名称包含()
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
