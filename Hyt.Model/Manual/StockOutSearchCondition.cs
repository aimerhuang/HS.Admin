using System;
using System.Collections.Generic;

namespace Hyt.Model
{
    /// <summary>
    /// 出库单搜索条件
    /// </summary>
    /// <remarks>2013-06-24 周瑜 创建</remarks>
    public class StockOutSearchCondition
    {
        /// <summary>
        /// 商品系统编号
        /// </summary>
        public int? ProductSysNo { get; set; }
        /// <summary>
        /// 商品erp编号
        /// </summary>
        public string ProductErpCode { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 出库开始日期
        /// </summary>
        public DateTime? StartStockOutDate { get; set; }

        /// <summary>
        /// 出库结束日期
        /// </summary>
        public DateTime? EndStockOutDate { get; set; }
        /// <summary>
        /// 出库单系统编号
        /// </summary>
        public int? StockOutSysNo { get; set; }

        /// <summary>
        /// 出库编号
        /// </summary>
        public int? WarehouseSysNo { get; set; }

        /// <summary>
        /// 订单系统编号
        /// </summary>
        public int? SoSysNo { get; set; }

        /// <summary>
        /// 订单系统编号列表
        /// </summary>
        public string OrderSysNoList { get; set; }

        /// <summary>
        /// 事务编号
        /// </summary>
        public string TransactionSysNo { get; set; }

        /// <summary>
        /// 发票系统编号 0： 无发票 > 0: 有发票
        /// </summary>
        public int? InvoiceSysNo { get; set; }

        /// <summary>
        /// 地区编号
        /// </summary>
        public int? AreaSysNo { get; set; }

        /// <summary>
        /// 出库单状态
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// 可发货状态
        /// </summary>
        public int? AwaitShipStatus { get; set; }

        /// <summary>
        /// 配送方式
        /// </summary>
        public int? DeliveryTypeSysNo { get; set; }

        /// <summary>
        /// SysNo过滤集
        /// </summary>
        public string SysNoFilter { get; set; }

        /// <summary>
        /// 当前页号
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// 快递单号
        /// </summary>
        public string ExpressNo { get; set; }

        /// <summary>
        /// 店铺系统编号
        /// </summary>
        public int? DsDealerMallSysNo { get; set; }

        /// <summary>
        /// 第三方订单编号
        /// </summary>
        public string ThirdPartyOrder { get; set; }

        /// <summary>
        /// 收件人姓名
        /// </summary>
        public string ReceiveName { get; set; }

        /// <summary>
        /// 会员帐号
        /// </summary>
        public string CustomerAccount { get; set; }
    }



    public static class StockOutSearchConditionList
    {

        /// <summary>
        /// 商品系统编号
        /// </summary>
        public static List<CBWhStockOut> RList { get; set; }

    }

    /// <summary>
    /// 出库单导出Excel所用实体
    /// </summary>
    /// 2017/10/12 吴琨  创建
    public class WhStockOutOutput
    {
        /// <summary>
        /// 出库单号
        /// </summary>
        public string 出库单号 { get; set; }

        /// <summary>
        /// 收货人
        /// </summary>
        public string 收货人 { get; set; }

        /// <summary>
        /// 仓库
        /// </summary>
        public string 仓库 { get; set; }

        /// <summary>
        /// 配送方式
        /// </summary>
        public string 配送方式 { get; set; }

        /// <summary>
        /// 应收金额
        /// </summary>
        public string 应收金额 { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string 创建时间 { get; set; }


        /// <summary>
        /// 是否开票
        /// </summary>
        public string 是否开票 { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public string 来源 { get; set; }


        /// <summary>
        /// 状态
        /// </summary>
        public string 状态 { get; set; }


    }
}
