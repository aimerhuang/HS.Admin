using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.TYO
{
    /// <summary>
    /// 广州跨境创建订单接口参数
    /// </summary>
    /// <remarks>2017-12-11 廖移凤 创建</remarks>
      [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class tmsWayBillNotifyRequest
    {
        /// <summary>
        /// 客户编码（测试用TESTSTD）
        /// </summary>
        public string clientId { get; set; }
        /// <summary>
        /// 申报海关（GZHG）
        /// </summary>
        public string customsID { get; set; }
        /// <summary>
        /// 交易订单编号，新增
        /// </summary>
        public string tradeId { get; set; }
        /// <summary>
        /// 物流订单编号
        /// </summary>
        public string orderCode { get; set; }
        /// <summary>
        /// 运单号
        /// </summary>
        public string waybill { get; set; }
        /// <summary>
        /// 总运单号
        /// </summary>
        public string totalWayBill { get; set; }
        /// <summary>
        /// 件数
        /// </summary>
        public int packNo { get; set; }
        /// <summary>
        /// 毛重，单位：KG
        /// </summary>
        public double grossWeigt { get; set; }
        /// <summary>
        /// 净重，单位：KG
        /// </summary>
        public double netWeight { get; set; }
        /// <summary>
        /// 主要货物名称
        /// </summary>
        public string goodsName { get; set; }
        /// <summary>
        /// 发件地区
        /// </summary>
        public string sendArea { get; set; }
        /// <summary>
        /// 收件地区
        /// </summary>
        public string consigneeArea { get; set; }
        /// <summary>
        /// 收件人名称
        /// </summary>
        public string consignee { get; set; }
        /// <summary>
        /// 收件人地址
        /// </summary>
        public string consigneeAddress { get; set; }
        /// <summary>
        /// 收件人联系方式
        /// </summary>
        public string consigneeTel { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>
        public string zipCode { get; set; }
        ///<summary>
        /// 关区代码
        /// </summary>
        public string customsCode { get; set; }
        /// <summary>
        /// 价值
        /// </summary>
        public double worth { get; set; }
        /// <summary>
        /// 进口日期
        /// </summary>
        public string importDateStr { get; set; }  
        /// <summary>
        /// 币制,142等
        /// </summary>
        public string currCode { get; set; }  
        /// <summary>
        /// 操作类型，A-新增；M-修改
        /// </summary>
        public string modifyMark { get; set; }
        /// <summary>
        /// 业务类型，1-一般进口；3-保税进口
        /// </summary>
        public string businessType { get; set; }
        /// <summary>
        /// 保价费，新增
        /// </summary>
        public double insuredFee { get; set; }
        /// <summary>
        /// 运费，新增
        /// </summary>
        public double freight { get; set; }
        /// <summary>
        /// 扩展字段，key=value格式，新增
        /// </summary>
        public string feature { get; set; }

      
    }
}