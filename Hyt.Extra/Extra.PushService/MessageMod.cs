using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.PushService
{
    public class MessageMod
    {
        /// <summary>
        /// 来源类型
        /// </summary>
        public string ComeType { get; set; }
        /// <summary>
        /// 信息类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 函数作用
        /// </summary>
        public string Function { get; set; }
        /// <summary>
        /// 门店编码
        /// </summary>
        public string ShopAppId { get; set; }
        /// <summary>
        /// 接收这的编号
        /// </summary>
        public string ReceiveId { get; set; }
        /// <summary>
        /// 内容消息
        /// </summary>
        public string Message { get; set; }

        public int OrderSysNo { get; set; }

        public string Receiver { get; set; }
        public string ReceiverTele { get; set; }
        public string ReceiverAddress { get; set; }
        public int TotalNumber { get; set; }
        public decimal TotalValue { get; set; }
        public string OrderItems { get; set; }
        public string CreateTime { get; set; }

        public string PayType { get; set; }
        public string DeliveryTime { get; set; }
        public int DsSysNo { get; set; }
    }
}
