using System;
using System.Collections.Generic;

namespace Hyt.Model
{
    /// <summary>
    /// 发票搜索条件
    /// </summary>
    /// <remarks>2013-07-09 周瑜 创建</remarks>
    public class InvoiceSearchCondition
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int? SysNo { get; set; }
        /// <summary>
        /// 订单系统编号
        /// </summary>
        public int? OrderSysNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public string TransactionSysNo { get; set; }
        /// <summary>
        /// 发票代码
        /// </summary>
        public string InvoiceCode { get; set; }

        /// <summary>
        /// 发票号码
        /// </summary>
        public string InvoiceNo { get; set; }

        /// <summary>
        /// 发票金额
        /// </summary>
        public decimal? InvoiceAmount { get; set; }

        /// <summary>
        /// 发票抬头
        /// </summary>
        public string InvoiceTitle { get; set; }

        /// <summary>
        /// 发票状态
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 发票类型系统编号
        /// </summary>
        public int? InvoiceTypeSysNo { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndDate { get; set; }

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
        /// 仓库系统编号
        /// </summary>
        public int? WarehouseSysNo { get; set; }

        /// <summary>
        /// 仓库编号集合
        /// </summary>
        public List<int> WarehouseSysNoList { get; set; }
    }
}
