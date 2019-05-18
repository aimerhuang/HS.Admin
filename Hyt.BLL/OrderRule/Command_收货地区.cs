using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL;
using Hyt.BLL.Web;
using Hyt.Model;
using SoOrderBo = Hyt.BLL.Order.SoOrderBo;

namespace Hyt.BLL.OrderRule
{
    /// <summary>
    /// 如果留言中有包含"、" 未处理转义字符
    /// </summary>
    public class Command_收货地区 : ICommand
    {

        public string[] keys { get; set; }
        public AreaType areaType { get; set; }

        public enum AreaType
        {
            省 = 1,
            市 = 2,
            区 = 3
        }
        public override bool Result(OrderData orderData)
        {
            if (keys.Length < 1) return false;
            if (orderData.Order == null || orderData.Order.ReceiveAddressSysNo == 0 || orderData.ReceiveArea == null) return false;
            string areaName;
            switch (areaType)
            {
                case AreaType.省:
                    areaName = orderData.ReceiveArea.Province;
                    break;
                case AreaType.市:
                    areaName = orderData.ReceiveArea.City;
                    break;
                case AreaType.区:
                    areaName = orderData.ReceiveArea.Region;
                    break;
                default:
                    areaName = orderData.ReceiveArea.Province;
                    break;
            }
            return keys.Any(k => areaName.Equals(k));
        }

        public override ICommand Parse(string command)
        {
            Dictionary<string, int> rules = new Dictionary<string, int>();
            rules.Add("收货地区省是", (int)AreaType.省);
            rules.Add("收货地区市是", (int)AreaType.市);
            rules.Add("收货地区区是", (int)AreaType.区);
            foreach (var item in rules)
            {
                var arg = this.GetArgumentKeys(item.Key, command);
                if (arg != null)
                {
                    return new Command_收货地区()
                    {
                        keys = arg,
                        areaType = (AreaType)item.Value
                    };
                }
            }
            return null;
        }
    }
}
