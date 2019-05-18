using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.PushService
{
    /// <summary>
    /// 消息推送商品集合
    /// </summary>
    public class MessageModItem
    {
        public string ProductName { get; set; }
        public string Quantity { get; set; }
        public string SalesAmount { get; set; }
    }
}
