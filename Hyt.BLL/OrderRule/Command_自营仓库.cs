using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.OrderRule
{
    public class Command_自营仓库 : ICommand
    {
        private readonly string rulename = "自营仓库";

        public override bool Result(OrderData orderData)
        {
            if (orderData.Warehouse == null) return false;
            return orderData.Warehouse.IsSelfSupport == 1;
        }

        public override ICommand Parse(string command)
        {
            return this.IsContainCommand(rulename, command) ? new Command_自营仓库() : null;

        }
    }
}
