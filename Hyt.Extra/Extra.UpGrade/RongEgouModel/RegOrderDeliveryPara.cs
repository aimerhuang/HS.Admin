using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.UpGrade.RongEgouModel
{
    /// <summary>
    /// 订单通知发货接口请求报文
    /// </summary>
    public class RegOrderDeliveryPara
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string order_id { get; set; }

        /// <summary>
        /// 物流公司编码（N）
        /// </summary>
        public string logistics_company { get; set; }

        /// <summary>
        /// 运单号（N）
        /// </summary>
        public string shipping_code { get; set; }

        /// <summary>
        /// 发货时间
        /// </summary>
        public string shipping_time { get; set; }

        /// <summary>
        /// 发货分组编号（N）
        /// </summary>
        public string shipping_num { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string notes { get; set; }

        /// <summary>
        /// 给买家的备注（N）
        /// </summary>
        public string merchant2member_notes { get; set; }

        /// <summary>
        /// 商品发货毛重（N）
        /// </summary>
        public string gross_weight { get; set; }

        /// <summary>
        /// 消费兑换码
        /// </summary>
        public string redeem_code { get; set; }

        /// <summary>
        /// 消费兑换码备注
        /// </summary>
        public string redeem_memo { get; set; }
    }
}
