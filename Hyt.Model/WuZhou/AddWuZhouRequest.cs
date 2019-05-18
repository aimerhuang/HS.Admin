using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Hyt.Model.WuZhou
{
    public class AddWuZhouRequest
    {
        /// <summary>
        /// 外部单据编号
        /// </summary>
        [Description("外部单据编号")]
        public string outer_code { get; set; }

        /// <summary>
        /// 货款合计
        /// </summary>
        [Description("货款合计")]
        public decimal goods_total { get; set; }

        /// <summary>
        /// 订单付款金额（含运费）
        /// </summary>
        [Description("订单付款金额（含运费）")]
        public decimal order_pay { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        [Description("运费")]
        public decimal logis_pay { get; set; }

        /// <summary>
        /// 优惠金额
        /// </summary>
        [Description("优惠金额")]
        public decimal favourable { get; set; }

        /// <summary>
        /// 货品总数
        /// </summary>
        [Description("货品总数")]
        public int item_count { get; set; }

        /// <summary>
        /// 订购人平台账号
        /// </summary>
        [Description("订购人平台账号")]
        public string ebp_account { get; set; }

        /// <summary>
        /// 订购人名称
        /// </summary>
        [Description("订购人名称")]
        public string buyer_name { get; set; }

        /// <summary>
        /// 订购人证件类型
        /// </summary>
        [Description("订购人证件类型")]
        public int buyer_idtype { get; set; }

        /// <summary>
        /// 订购人证件号码
        /// </summary>
        [Description("订购人证件号码")]
        public string buyer_idnumber { get; set; }

        /// <summary>
        /// 订购人联系方式
        /// </summary>
        [Description("订购人联系方式")]
        public string buyer_tel { get; set; }

        /// <summary>
        /// 收货人名称
        /// </summary>
        [Description("订收货人名称")]
        public string consignee { get; set; }

        /// <summary>
        /// 收货人邮编
        /// </summary>
        [Description("收货人邮编")]
        public string consignee_postcode { get; set; }

        /// <summary>
        /// 收货人联系方式
        /// </summary>
        [Description("收货人联系方式")]
        public string consignee_tel { get; set; }

        /// <summary>
        /// 收货人所在省
        /// </summary>
        [Description("收货人所在省")]
        public string consignee_province { get; set; }

        /// <summary>
        /// 收货人所在市
        /// </summary>
        [Description("收货人所在市")]
        public string consignee_city { get; set; }

        /// <summary>
        /// 收货人所在区、县
        /// </summary>
        [Description("收货人所在区、县")]
        public string consignee_district { get; set; }

        /// <summary>
        /// 收货地址
        /// </summary>
        [Description("收货地址")]
        public string consignee_addr { get; set; }

        /// <summary>
        /// 收货人emaill
        /// </summary>
        [Description("收货人emaill")]
        public string consignee_email { get; set; }

        /// <summary>
        /// 电商企业海关代码
        /// </summary>
        [Description("电商企业海关代码")]
        public string ebc_code { get; set; }

        /// <summary>
        /// 电商平台海关代码
        /// </summary>
        [Description("电商平台海关代码")]
        public string ebp_code { get; set; }

        /// <summary>
        /// 支付企业代码
        /// </summary>
        [Description("支付企业代码")]
        public string pay_code { get; set; }

        /// <summary>
        /// 支付信息编号
        /// </summary>
        [Description("支付信息编号")]
        public string payment_no { get; set; }

        /// <summary>
        /// 是否需要五洲传输订单报文
        /// </summary>
        [Description("是否需要五洲传输订单报文")]
        public int is_ordermsg { get; set; }

        /// <summary>
        /// 是否需要五洲传输支付报文
        /// </summary>
        [Description("是否需要五洲传输支付报文")]
        public int is_paymsg { get; set; }

        /// <summary>
        /// 是否需要五洲传输运单报文
        /// </summary>
        [Description("是否需要五洲传输运单报文")]
        public int is_logismsg { get; set; }

        /// <summary>
        /// 是否需要五洲传输清单报文
        /// </summary>
        [Description("是否需要五洲传输清单报文")]
        public int is_invtmsg { get; set; }

        /// <summary>
        /// 快递公司名称
        /// </summary>
        [Description("快递公司名称")]
        public string express_name { get; set; }

        /// <summary>
        /// 快递单号
        /// </summary>
        [Description("快递单号")]
        public string logis_num { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        public string note { get; set; }

        /// <summary>
        /// sku明细 item_list
        /// </summary>
        [Description("sku明细 item_list")]
        public IList<Item_Lists> item_list { get; set; }

    }
}
