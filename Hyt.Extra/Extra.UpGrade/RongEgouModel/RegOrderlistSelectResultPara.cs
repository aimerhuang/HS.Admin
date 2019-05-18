using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.UpGrade.RongEgouModel
{
    /// <summary>
    /// 订单详情查询接口请求成功返回报文
    /// </summary>
    /// <remarks>2018-03-19 罗熙 创建</remarks>
    public class RegOrderlistSelectResultPara
    {
        /// <summary>
        /// 订单列表
        /// </summary>
        public ResultOrderlist order_list { get; set; }
    }

    /// <summary>
    /// 订单列表
    /// </summary>
    public class ResultOrderlist
    {
        /// <summary>
        /// 单个订单
        /// </summary>
        public List<ResultOrder> order { get; set; }
    }

    /// <summary>
    /// 单个订单
    /// </summary>
    public class ResultOrder
    {
        /// <summary>
        /// 订单id
        /// </summary>
        public string order_id { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public string order_modify_time { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public string order_status { get; set; }

        /// <summary>
        /// 买方订单备注（N）
        /// </summary>
        public string order_buyer_remark { get; set; }

        /// <summary>
        /// 卖方订单备注（N）
        /// </summary>
        public string order_seller_remark { get; set; }

        /// <summary>
        /// 卖方给买方的备注（N）
        /// </summary>
        public string merchant2member_remark { get; set; }

        /// <summary>
        /// 买方ID
        /// </summary>
        public string order_buyer_id { get; set; }

        /// <summary>
        /// 买方登录名（N）
        /// </summary>
        public string order_buyer_username { get; set; }

        /// <summary>
        /// 买方名称（N）
        /// </summary>
        public string order_buyer_name { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        public string order_create_time { get; set; }

        /// <summary>
        /// 订单总金额
        /// </summary>
        public decimal order_amount { get; set; }

        /// <summary>
        /// 积分抵扣金额（N）
        /// </summary>
        public decimal order_credit_amount { get; set; }

        /// <summary>
        /// 积分清算金额（N）
        /// </summary>
        public decimal credit_liquid_amount { get; set; }

        /// <summary>
        /// 其他优惠金额（N）
        /// </summary>
        public decimal order_other_discount { get; set; }

        /// <summary>
        /// 下单渠道（1：PC，2：手机，3：PAD）
        /// </summary>
        public int order_channel { get; set; }

        /// <summary>
        /// 电子券抵扣金额（N）
        /// </summary>
        public decimal order_coupon_amount { get; set; }

        /// <summary>
        /// 优惠分期（N）
        /// </summary>
        public string order_ins_num { get; set; }

        /// <summary>
        /// 订单旗标 （01-红旗，02-绿旗，03-橙旗，04-蓝旗，05-粉旗，06-青旗，07-黄旗，08-紫旗，09-灰旗）
        /// </summary>
        public string order_flag_color { get; set; }

        /// <summary>
        /// 优惠券列表
        /// </summary>
        public Discounts discounts { get; set; }

        /// <summary>
        /// 商品列表节点
        /// </summary>
        public ResultProducts products { get; set; }
    }

    /// <summary>
    /// 优惠券列表
    /// </summary>
    public class Discounts
    {
        /// <summary>
        /// 单个优惠信息节点
        /// </summary>
        public List<Discount> discount { get; set; }
    }

    /// <summary>
    /// 单个优惠信息节点
    /// </summary>
    public class Discount
    {
        /// <summary>
        /// 优惠类型 （03=人为优惠（针对订单的优惠）, 06:人工优惠（针对运费的优惠），07：包邮（不可与其他活动叠加），08：满送（针对满足条件的订单自动减价，商户活动），09：满减（针对满足条件的订单自动减价，商城活动），13：包邮（包邮活动，可与其他活动叠加），18：首单立减。优惠类型可能随时修改和增加，建议商户将优惠金额累加算出总优惠，不要针对某一项优惠做任何特殊处理；每一种类型的优惠只会出现一次；）
        /// </summary>
        public string discount_type { get; set; }

        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal discount_amount { get; set; }
    }

    /// <summary>
    /// 订单商品信息
    /// </summary>
    public class ResultProducts
    {
        /// <summary>
        /// 单个订单商品信息
        /// </summary>
        public List<ResultProduct> product { get; set; }
    }

    /// <summary>
    /// 单个订单商品信息
    /// </summary>
    public class ResultProduct
    {
        /// <summary>
        /// 商品id
        /// </summary>
        public string product_id { get; set; }

        /// <summary>
        /// 商品skuID
        /// </summary>
        public string product_sku_id { get; set; }

        /// <summary>
        /// 商品商户编码
        /// </summary>
        public string product_code { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string product_name { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        public int product_number { get; set; }

        /// <summary>
        /// 商品价格
        /// </summary>
        public decimal product_price { get; set; }

        /// <summary>
        /// 商品优惠金额（N）（商品优惠金额，暂时无用）
        /// </summary>
        public decimal product_discount { get; set; }

        /// <summary>
        /// 商品属性
        /// </summary>
        public string product_prop_info { get; set; }

        /// <summary>
        /// 商品退款进度（商品退款进度，有“-”、“已申请退款”、“退款处理中”、“退款完成”。“-”表示不存在有效退款单，“已申请退款”表示商户尚未审核该商品的退款单，“退款处理中”表示该商品的退款单正在处理中，“退款完成”表示该商品已经完成退款）
        /// </summary>
        public string refund_process { get; set; }

        /// <summary>
        /// 商品申请退款数量
        /// </summary>
        public int refund_num { get; set; }

        /// <summary>
        /// 活动列表节点
        /// </summary>
        public Activities activities { get; set; }

        /// <summary>
        /// 搭售商品列表
        /// </summary>
        public ResultOrderTringproducts tringproducts { get; set; }

        /// <summary>
        /// 发票节点
        /// </summary>
        public Invoice invoice { get; set; }

        /// <summary>
        /// 支付节点
        /// </summary>
        public Payment payment { get; set; }

        /// <summary>
        /// 收货信息节点
        /// </summary>
        public Consignee consignee { get; set; }
    }

    /// <summary>
    /// 活动列表节点
    /// </summary>
    public class Activities
    {
        /// <summary>
        /// 单个活动列表节点
        /// </summary>
        public Activity activity { get; set; }
    }

    /// <summary>
    /// 单个活动列表节点
    /// </summary>
    public class Activity
    {
        /// <summary>
        /// 活动id
        /// </summary>
        public string activity_id { get; set; }

        /// <summary>
        /// 活动类型
        /// </summary>
        public string activity_type { get; set; }

        /// <summary>
        /// 活动名称
        /// </summary>
        public string activity_name { get; set; }
    }

    /// <summary>
    /// 搭售商品列表
    /// </summary>
    public class ResultOrderTringproducts
    {
        /// <summary>
        /// 单个搭售商品
        /// </summary>
        public List<ResultOrderTringproduct> tringproduct { get; set; }
    }

    /// <summary>
    /// 单个搭售商品
    /// </summary>
    public class ResultOrderTringproduct
    {
        /// <summary>
        /// 商品id
        /// </summary>
        public string product_id { get; set; }

        /// <summary>
        /// 商品skuID
        /// </summary>
        public string product_sku_id { get; set; }

        /// <summary>
        /// 商品商户编码
        /// </summary>
        public string product_code { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string product_name { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        public int product_number { get; set; }

        /// <summary>
        /// 商品价格
        /// </summary>
        public decimal product_price { get; set; }

        /// <summary>
        /// 商品属性（N）
        /// </summary>
        public string product_prop_info { get; set; }

        /// <summary>
        /// 商品退款进度
        /// </summary>
        public string refund_process { get; set; }

        /// <summary>
        /// 商品申请退款数量
        /// </summary>
        public int refund_num { get; set; }

        /// <summary>
        /// 赠品列表
        /// </summary>
        public Giftproducts giftproducts { get; set; }
    }

    /// <summary>
    /// 赠品列表
    /// </summary>
    public class Giftproducts
    {
        public List<Giftproduct> giftproduct { get; set; }
    }

    /// <summary>
    /// 单个赠品
    /// </summary>
    public class Giftproduct
    {
        /// <summary>
        /// 商品skuID
        /// </summary>
        public string product_sku_id { get; set; }

        /// <summary>
        /// 商品商户编码
        /// </summary>
        public string product_code { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string product_name { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        public int product_number { get; set; }
    }

    public class Invoice
    {
        /// <summary>
        /// 发票类型票
        /// </summary>
        public List<InvoiceType> invoice_type { get; set; }
    }

    /// <summary>
    /// 发票类型票
    /// </summary>
    public class InvoiceType
    {
        /// <summary>
        /// 发票抬头
        /// </summary>
        public string invoice_title { get; set; }

        /// <summary>
        /// 发票内容
        /// </summary>
        public string invoice_content { get; set; }

        /// <summary>
        /// 注册地址
        /// </summary>
        public string register_address { get; set; }

        /// <summary>
        /// 注册电话
        /// </summary>
        public string register_tel { get; set; }

        /// <summary>
        /// 开户银行
        /// </summary>
        public string deposit_bank { get; set; }

        /// <summary>
        /// 银行账户
        /// </summary>
        public string bank_account { get; set; }

        /// <summary>
        /// 纳税人识别号
        /// </summary>
        public string taxpayer_id { get; set; }
    }

    /// <summary>
    /// 支付节点
    /// </summary>
    public class Payment
    {
        /// <summary>
        /// 支付时间
        /// </summary>
        public string order_pay_time { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal order_pay_amount { get; set; }

        /// <summary>
        /// 支付系统号
        /// </summary>
        public string order_pay_sys { get; set; }

        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal order_discount_amount { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        public decimal order_freight { get; set; }

        /// <summary>
        /// 流水号
        /// </summary>
        public string pay_serial { get; set; }

        /// <summary>
        /// 电子券节点
        /// </summary>
        public Coupons coupons { get; set; }

    }

    /// <summary>
    /// 电子券节点
    /// </summary>
    public class Coupons
    {
        /// <summary>
        /// 单个电子券节点
        /// </summary>
        public List<Coupon> coupon { get; set; }
    }

    /// <summary>
    /// 单个电子券节点
    /// </summary>
    public class Coupon
    {
        /// <summary>
        /// 电子券编号
        /// </summary>
        public string coupon_id { get; set; }

        /// <summary>
        /// 电子券活动编号
        /// </summary>
        public string coupon_promo_id { get; set; }

        /// <summary>
        /// 电子券活动名称
        /// </summary>
        public string coupon_promo_name { get; set; }

        /// <summary>
        /// 电子券初始金额
        /// </summary>
        public decimal coupon_org_amount { get; set; }

        /// <summary>
        /// 电子券支付使用金额
        /// </summary>
        public decimal coupon_use_amount { get; set; }

        /// <summary>
        /// 电子券类型 （01-通用券（商城），02-定向券（商城、商户），03-专户券（商户））
        /// </summary>
        public string coupon_type { get; set; }
    }

    /// <summary>
    /// 收货信息节点
    /// </summary>
    public class Consignee
    {
        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string consignee_name { get; set; }

        /// <summary>
        /// 收货地址省名
        /// </summary>
        public string consignee_province { get; set; }

        /// <summary>
        /// 收货地址省份编码
        /// </summary>
        public string consignee_province_id { get; set; }

        /// <summary>
        /// 收货地址城市名
        /// </summary>
        public string consignee_city { get; set; }

        /// <summary>
        /// 收货地址城市编码
        /// </summary>
        public string consignee_city_id { get; set; }

        /// <summary>
        /// 收货地址区县名
        /// </summary>
        public string consignee_district { get; set; }

        /// <summary>
        /// 收货地址区县编码
        /// </summary>
        public string consignee_district_id { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string consignee_address { get; set; }

        /// <summary>
        /// 收货地址编码
        /// </summary>
        public string consignee_zipcode { get; set; }

        /// <summary>
        /// 收货完整地址
        /// </summary>
        public string consignee_total_address { get; set; }

        /// <summary>
        /// 收货人手机
        /// </summary>
        public string consignee_mobile { get; set; }

        /// <summary>
        /// 收货人固定电话
        /// </summary>
        public string consignee_phone { get; set; }

        /// <summary>
        /// 配送时间要求（1仅工作日收货，2仅节假日收货，3工作日与节假日均可收货）
        /// </summary>
        public string consignee_time { get; set; }

        /// <summary>
        /// 收货人身份证号
        /// </summary>
        public string consignee_idcardnum { get; set; }

        /// <summary>
        /// 收货人邮箱
        /// </summary>
        public string consignee_email { get; set; }

        /// <summary>
        /// 商户自定义参数（如QQ号等）
        /// </summary>
        public string merDefined1 { get; set; }

        /// <summary>
        /// 商户自定义2
        /// </summary>
        public string merDefined2 { get; set; }

        /// <summary>
        /// 商户自定义3
        /// </summary>
        public string merDefined3 { get; set; }
    }
}
