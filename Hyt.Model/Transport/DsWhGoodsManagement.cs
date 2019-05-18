using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transport
{
    public  class WhGoodsManagementGroup
    {
        public string CreateTime { get; set; }
        public string OrderBatchNum { get; set; }
        public string CustomerCode { get; set; }
        public int OrderCount { get; set; }

        public string CusCode { get; set; }
    }
    /// <summary>
    /// 包裹信息管理扩展实体
    /// </summary>
    /// <remarks>
    /// 2016-5-17 杨云奕 添加
    /// </remarks>
    public class CBDsWhGoodsManagement:DsWhGoodsManagement
    {
        public string CalculateWeight { get; set; }
        /// <summary>
        /// 税号
        /// </summary>
        public string TaxCode { get; set; }
        /// <summary>
        /// idcard
        /// </summary>
        public string IDCardImg { get; set; }
        public decimal ToTalValue { get; set; }
        /// <summary>
        /// 明细集合
        /// </summary>
        public List<DsWhGoodsManagementList> ModList { get; set; }
        /// <summary>
        /// 总运单
        /// </summary>
        public List<DsWhTotalWaybill> WayBillList { get; set; }
        /// <summary>
        /// 物流信息记录
        /// </summary>
        public List<CBDsWhHistory> HistoryList { get; set; }
        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
    }

    /// <summary>
    /// 包裹信息管理
    /// </summary>
    /// <remarks>
    /// 2016-5-16 杨云奕 添加
    /// </remarks>
    public  class DsWhGoodsManagement
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 报关单号
        /// </summary>
        public string CustomsNum { get; set; }
        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNumber { get; set; }
        /// <summary>
        /// 总运单号
        /// </summary>
        public string TotalWaybillNum { get; set; }
        /// <summary>
        /// 袋号
        /// </summary>
        public string BagNumber { get; set; }
        /// <summary>
        /// 快递单号
        /// </summary>
        public string CourierNumber { get; set; }
        /// <summary>
        /// 服务类型
        /// </summary>
        public string ServiceType { get; set; }
        /// <summary>
        /// 发货人
        /// </summary>
        public string SendUser { get; set; }
        /// <summary>
        /// 发货地址
        /// </summary>
        public string SendAddress { get; set; }
        /// <summary>
        /// 发货人号码
        /// </summary>
        public string SendTelephone { get; set; }
        /// <summary>
        /// 收货人
        /// </summary>
        public string Receipter { get; set; }
        /// <summary>
        /// 收货人身份证
        /// </summary>
        public string IDCard { get; set; }
        /// <summary>
        /// 收货人联系电话
        /// </summary>
        public string ReceiptTele { get; set; }
        /// <summary>
        /// 收货人所在城市
        /// </summary>
        public string ReceiptCity { get; set; }
        /// <summary>
        /// 收货人编码
        /// </summary>
        public string ReceiptMall { get; set; }
        /// <summary>
        /// 收货人地址
        /// </summary>
        public string ReceiptAddress { get; set; }
        /// <summary>
        /// 件内名称
        /// </summary>
        public string ReceiptPageName { get; set; }
        /// <summary>
        /// 保内数量
        /// </summary>
        public string ReceiptPageNum { get; set; }
        /// <summary>
        /// 保价金额
        /// </summary>
        public decimal Valuation { get; set; }
        /// <summary>
        /// 包裹重量
        /// </summary>
        public decimal WeightValue { get; set; }
        /// <summary>
        /// 币别
        /// </summary>
        public string Currency { get; set; }
        /// <summary>
        /// 状态码类型
        /// </summary>
        public string StatusCode { get; set; }
        /// <summary>
        /// 状态码类型
        /// </summary>
        public string PayStatus { get; set; }
        /// <summary>
        /// 客户编码
        /// </summary>
        public string CustomerCode { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        public string AssNumber { get; set; }
        public string Dis { get; set; }
        /// <summary>
        /// 客户参考码
        /// </summary>
        public string CustomerCheckCode { get; set; }
        /// <summary>
        /// 海淘物流编号
        /// </summary>
        public string HTLogistics { get; set; }
    }
}
