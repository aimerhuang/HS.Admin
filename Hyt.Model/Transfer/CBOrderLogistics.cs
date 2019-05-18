using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    public class CBOrderLogistics
    {
        /// <summary>
        /// 订单明细
        /// </summary>
        public SoOrder Order = new SoOrder();
        /// <summary>
        /// 订单系统编号
        /// </summary>
        public int orderId { get; set; }
        /// <summary>
        /// 推送编码
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 总运单号
        /// </summary>
        public string pfreight_no { get; set; }

        /// <summary>
        /// 快件单号
        /// </summary>
        public string express_num { get; set; }

        /// <summary>
        /// 发件人姓名
        /// </summary>
        public string sender_name { get; set; }

        /// <summary>
        /// 发件人城市
        /// </summary>
        public string sender_city { get; set; }

        /// <summary>
        /// 发件人地址
        /// </summary>
        public string sender_address { get; set; }

        /// <summary>
        /// 发件人电话
        /// </summary>
        public string sender_phone { get; set; }

        /// <summary>
        /// 发件人国别
        /// </summary>
        public string sender_country_code { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public string goods_gweight { get; set; }
    }
}
