using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.ExpressList
{
    /// <summary>
    /// 快递100 电子面单接口参数
    /// </summary>
    /// <remarks>2017-11-25 廖移凤 创建</remarks>
    public class KdOrderParam
    {
        public KdOrderParam()
        {
            partnerId = "";
            partnerKey = "";
            net = "";
            kuaidicom = "";
            kuaidinum = "";
            orderId = 0;
            cargo = "";
            volumn = "";
            count = 0;
            weight = 0.0;
            remark = "";
            payType = "";
            expType = "";
            valinsPay = 0.0;
            collectione = 0.0;
            needChild = 0;
            needBack = 0;
            needTemplate = 0;
            recMan = new RecMan();
            sendMan = new RecMan();
        }
        /// <summary>
        /// 电子面单客户账户或月结账号，需向快递公司在贵司当地的网点申请；若已和快递100超市业务合作，则可不填。顺丰、EMS的可输入月结账号；若所选快递公司为宅急送（即kuaidicom字段为zhaijisong），则此项可不填
        /// </summary>
        public string partnerId { get; set; }
        /// <summary>
        ///电子面单密码，需向快递公司在贵司当地的网点申请；若已和快递100超市业务合作，则可不填。顺丰、EMS的如果partnerId填月结账号，则此字段不填；若所选快递公司为宅急送（即kuaidicom字段为zhaijisong），则此项可不填
        /// </summary>
        public string partnerKey { get; set; }
        /// <summary>
        /// 收件网点名称,由快递公司当地网点分配，若已和快递100超市业务合作，则可不填。顺丰、EMS的如果partnerId填月结账号，则此字段不填；若所选快递公司为宅急送（即kuaidicom字段为zhaijisong），则此项可不填
        /// </summary>
        public string net { get; set; }
        /// <summary>
        ///快递公司的编码，一律用小写字母，见《快递公司编码》,必填
        /// </summary>
        public string kuaidicom { get; set; }
        /// <summary>
        /// 快递公司的编码，一律用小写字母，见《快递公司编码》,必填
        /// </summary>
        public string kuaidinum { get; set; }
        /// <summary>
        ///  贵司内部自定义的订单编号，需要保证唯一性，非必填
        /// </summary>
        public int orderId { get; set; }
        /// <summary>
        /// 物品名称，非必填
        /// </summary>
        public string cargo { get; set; }
        /// <summary>
        /// 物品总数量，必填；如果需要子单（指同一个订单打印出多张电子面单，即同一个订单返回多个面单号），needChild = 1、count 需要大于1，如count = 2 则一个主单 一个子单，count = 3则一个主单 二个子单；返回的子单号码见返回结果的childNum字段
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// 物品总重量KG，必填
        /// </summary>
        public double weight { get; set; }
        /// <summary>
        /// 物品总体积，CM*CM*CM，非必填
        /// </summary>
        public string volumn { get; set; }
        /// <summary>
        /// 支付方式：SHIPPER:寄方付（默认）、CONSIGNEE:到付、MONTHLY:月结、THIRDPARTY:第三方支付，非必填
        /// </summary>
        public string payType { get; set; }
        /// <summary>
        /// 快递类型:标准快递（默认）、顺丰特惠、EMS经济，非必填
        /// </summary>
        public string expType { get; set; }
        /// <summary>
        /// 备注,非必填
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 保价额度，非必填
        /// </summary>
        public double valinsPay { get; set; }
        /// <summary>
        /// 代收货款额度，非必填
        /// </summary>
        public double collectione { get; set; }
        /// <summary>
        /// 是否需要子单：1:需要、0:不需要(默认) ，String类型，非必填；如果需要子单（指同一个订单打印出多张电子面单，即同一个订单返回多个面单号），needChild = 1、count 需要大于1，如count = 2 一个主单 一个子单，count = 3 一个主单 二个子单，返回的子单号码见返回结果的childNum字段
        /// </summary>
        public int needChild { get; set; }
        /// <summary>
        /// 是否需要回单：1:需要、 0:不需要(默认) ，String类型，非必填；返回的回单号见返回结果的returnNum字段
        /// </summary>
        public int needBack { get; set; }
        /// <summary>
        /// 是否需要打印模板：1:需要、 0 不需要(默认) ，如果需要，则返回要打印的模版的HTML代码，贵司可以直接将之显示到IE等浏览器，然后通过浏览器进行打印,非必填
        /// </summary>
        public int needTemplate { get; set; }
        /// <summary>
        /// 收货人
        /// </summary>
        public RecMan recMan { get; set; }
        /// <summary>
        /// 寄件人
        /// </summary>
        public RecMan sendMan { get; set; }

    }
}
