using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.OrderRule
{
    public class Command_仓库名称是 : ICommand
    {
        private readonly string rulename = "仓库名称是";
        /// <summary>
        /// 关键字
        /// </summary>
        public string[] keys { get; set; }
        public override bool Result(OrderData orderData)
        {
            bool flg = false;
            if (orderData != null && orderData.Order != null && keys != null && !string.IsNullOrEmpty(orderData.WarehouseName))
            {
                flg = keys.Contains(orderData.WarehouseName);
            }
            return flg;
        }

        public override ICommand Parse(string command)
        {
            var arg = this.GetArgumentKeys(rulename, command);
            if (arg != null)
            {
                return new Command_仓库名称是()
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
