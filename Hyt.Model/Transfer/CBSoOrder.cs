using System;
namespace Hyt.Model
{
    /// <summary>
    /// 订单池客服实体类
    /// </summary>
    /// <remarks>
    /// 2013/6/19 余勇 创建 
    /// </remarks>
    /// <remarks>
    /// 2016-04-05 杨云奕 修改
    /// </remarks>
    [Serializable]
    public partial class CBSoOrder 
    {
        #region 自定义字段
        /// <summary>
        /// 订单销售单号
        /// </summary>
        public int SysNo { set; get; }

        /// <summary>
        /// 状态：待审核（10）、待支付（20）、待出库（30）、部
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 仓库号
        /// </summary>
        public string WarehouseName { set; get; }

        /// <summary>
        /// 后台仓库名称
        /// </summary>
        public string BackWarehouseName { set; get; }

        /// <summary>
        /// 出库仓库号
        /// </summary>
        public int WhStockOutSysNo { set; get; }

        /// <summary>
        /// 会员姓名
        /// </summary>
        public string CustomerName { set; get; }

        /// <summary>
        /// 收货人
        /// </summary>
        public string ReceiveName { set; get; }

        /// <summary>
        /// 收货电话
        /// </summary>
        public string ReceiveTel { set; get; }

        /// <summary>
        /// 配送方式
        /// </summary>
        public string DeliveryTypeName { set; get; }

        /// <summary>
        /// 快递公司编号
        /// </summary>
        public string OverseaCarrier { set; get; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public string PaymentName { set; get; }

        /// <summary>
        /// 销售单总金额
        /// </summary>
        public decimal OrderAmount { set; get; }

        /// <summary>
        /// 积分
        /// </summary>
        public int Point { set; get; }

        /// <summary>
        /// 销售单来源：PC网站（10）、信营全球购B2B2C3G网站（15
        /// </summary>
        public int OrderSource { get; set; }

        /// <summary>
        /// 订购时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 出库时间
        /// </summary>
        public DateTime StockOutDate { get; set; }

        /// <summary>
        /// 销售单审核时间
        /// </summary>
        public DateTime AuditorDate { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdateDate { get; set; }

        /// <summary>
        /// 分配人
        /// </summary>
        public string AssignName { set; get; }

        /// <summary>
        /// 申领人
        /// </summary>
        public string ApplyName { set; get; }

        /// <summary>
        /// 审核人
        /// </summary>
        public string AuditorName { set; get; }

        /// <summary>
        /// 订单池编号
        /// </summary>
        public string JobSysNo { set; get; }

        /// <summary>
        /// 快速查询条件
        /// </summary>
        public string Condition { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { set; get; }

        /// <summary>
        /// 当前页
        /// </summary>
        public int CurrentPage { set; get; }

        /// <summary>
        /// 当前页
        /// </summary>
        public int id { set; get; }

        /// <summary>
        /// 订单人帐号
        /// </summary>
        public string CustomerAccount { get; set; }

        /// <summary>
        /// 会员编号
        /// </summary>
        public int? CustomerSysNo { get; set; }

        /// <summary>
        /// 总金额中现金支付部分
        /// </summary>
        public decimal CashPay { get; set; }

        /// <summary>
        /// 总金额中惠源币支付部分
        /// </summary>
        public decimal CoinPay { get; set; }

        /// <summary>
        /// 订单创建人
        /// </summary>
        public string OrderCreator { get; set; }

        /// <summary>
        /// 客户编号
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// 支付状态 
        /// </summary>
        public int PayStatus { get; set; }
       

        /// <summary>
        /// 订单状态
        /// </summary>
        public string OnlineStatus { get; set; }
        /// <summary>
        /// 订单事务编号
        /// </summary>
        /// 2013-11-28 黄志勇 创建
        public string TransactionSysNo { get; set; }

        /// <summary>
        /// 分销商城类型系统编号
        /// </summary>
        /// 2013-11-28 黄志勇 创建
        public int MallTypeSysNo { get; set; }

        /// <summary>
        /// 图片标识
        /// </summary>
        public string ImgFlag { get; set; }

        /// <summary>
        /// 仓库类型
        /// </summary>
        /// 2015-09-2 王耀发 创建
        public int WarehouseType { get; set; }
        /// <summary>
        /// 物流
        /// </summary>
        public int Logistics { get; set; }
        /// <summary>
        /// 海关
        /// </summary>
        public int Customs { get; set; }
        /// <summary>
        /// 商检
        /// </summary>
        public int Inspection { get; set; }
        /// <summary>
        /// 推送状态 
        /// 2015-09-2 王耀发 创建
        /// </summary>
        public int SendStatus { get; set; }
        /// <summary>
        /// 跨境物流推送状态
        /// </summary>
        public int CBLogisticsSendStatus { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 分销商
        /// </summary>
        public string DealerName { set; get; }
        /// <summary>
        /// 支付方式编号
        /// </summary>
        public int PayTypeSysNo { get; set; }
        /// <summary>
        /// 支付信息系海关推送状态
        /// </summary>
        public int CustomsPayStatus { get; set; }
        /// <summary>
        /// 订单商检推送状态
        /// </summary>
        public int TradeCheckStatus { get; set; }
        /// <summary>
        /// 订单海关推送状态
        /// </summary>
        public int CustomsStatus { get; set; }
        /// <summary>
        /// 仓库编号
        /// </summary>
        public int DefaultWarehouseSysNo { get; set; }
        /// <summary>
        /// 搜索条件选中的分销商
        /// </summary>
        public int SelectedDealerSysNo { get; set; }
        /// <summary>
        /// 搜索条件选中的代理商
        /// </summary>
        public int SelectedAgentSysNo { get; set; }
        /// <summary>
        /// 是否绑定所有仓库
        /// </summary>
        public bool HasAllWarehouse { get; set; }
        /// <summary>
        /// 销售单支付时间
        /// </summary>
        public DateTime? PaymentDate { get; set; }

        /// <summary>
        /// 广州机场商检推送状态 
        /// 2016-4-15 王耀发 创建
        /// </summary>
        public int GZJCStatus { get; set; }

        /// <summary>
        /// 南沙商检推送状态 
        /// 2016-4-15 王耀发 创建
        /// </summary>
        public int NsStatus { get; set; }

        /// <summary>
        /// 供应链编号
        /// </summary>
        public int Supply { get; set; }
        #endregion

        #region 字段 杨云奕 2016-04-05
        public string StreetAddress { get; set; }

        public string AreaInfo { get; set; }
        /// <summary>
        /// 商品
        /// </summary>
        public decimal TotalProductNumber { get; set; }
        public string StrCreateTime { get; set; }
        #endregion

        #region 字段 罗勤尧 2017-05-03
        /// <summary>
        /// 第三方订单号
        /// </summary>
        public string MallOrderId { get; set; }

        /// <summary>
        /// 第三方支付时间
        /// </summary>
        public string PayTime { get; set; }

        /// <summary>
        /// 配送备注
        /// </summary>
        public string DeliveryRemarks { get; set; }
        
     
        #endregion
    }
}

