using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transport
{
    /// <summary>
    /// 出库单操作扩展实体
    /// </summary>
    /// <remarks>
    /// 2016-5-17 杨云奕 添加
    /// </remarks>
    public class CBDsWhOutStock : DsWhOutStock
    {
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        /// <summary>
        /// 出库商品实体集合
        /// </summary>
        public List<DsWhOutStockList> ModList { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        public string searchText { get; set; }
    }
    /// <summary>
    /// 出库单操作
    /// </summary>
    /// <remarks>
    /// 2016-5-16 杨云奕 添加
    /// </remarks>
    public class DsWhOutStock
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 出库批次编号
        /// </summary>
        public string BatchOutNumber { get; set; }
        /// <summary>
        /// 客户编码
        /// </summary>
        public string CustomerCode { get; set; }
        /// <summary>
        /// 总包裹数量
        /// </summary>
        public int TotalPageNum { get; set; }
        /// <summary>
        /// 总出库重量
        /// </summary>
        public decimal TotalWeight { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Dis { get; set; }
        /// <summary>
        /// 总费用
        /// </summary>
        public decimal TotalValue { get; set; }

        public DateTime OutStockTime { get; set; }
        public string StatusCode { get; set; }

        /// <summary>
        /// 客户编码
        /// </summary>
        public string CusCode { get; set; }
    }
}
