using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.FinancialStatistics
{
    public class StatisticsDataMod 
    {
        public int SysNo { get; set; }
        public string DataInfo { get; set; }
        public decimal Amount { get; set; }
    }

    public class StatisticsType
    {
        public decimal CashPay { get; set; }
        public string PaymentName { get; set; }
    }
    public class AllStatistics
    {
        public decimal Cash { get; set; }
        /// <summary>
        /// 支付宝
        /// </summary>
        public decimal AliPay { get; set; }
        /// <summary>
        /// 微信
        /// </summary>
        public decimal WebXin { get; set; }
        /// <summary>
        /// 网银
        /// </summary>
        public decimal Bank { get; set; }
        /// <summary>
        /// 实体店
        /// </summary>
        public decimal StoreStock { get; set; }
        /// <summary>
        /// 报税商品
        /// </summary>
        public decimal BaoShui { get; set; }
        ///退货支出
        public decimal RetPrice { get; set; }
        ///物流支出
        public decimal Debang { get; set; }
        /// <summary>
        /// 顺丰物流费用
        /// </summary>
        public decimal SF { get; set; }
        /// <summary>
        /// 心仪物流费用
        /// </summary>
        public decimal XinYiLogistics { get; set; }
        ///行邮税支出
        public decimal HaiGuanPostTax { get; set; }
    }
    /// <summary>
    /// 统计表扩展实体
    /// </summary>
    public class CBFnStatistics : FnStatistics
    {
        public List<CBFnSalesOrSpendStatistics> SaleOrSpendList { get; set; }
    }

    /// <summary>
    /// 财务统计表
    /// </summary>
    public class FnStatistics
    {
        public int SysNo { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime BindTime { get; set; }
        public decimal TotalSalesAmount { get; set; }
        public decimal TotalSpendAmount { get; set; }
        public decimal TotalProfit { get; set; }
    }
}
