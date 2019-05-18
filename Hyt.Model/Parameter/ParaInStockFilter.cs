using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 入库单筛选字段
    /// </summary>
    /// <remarks>2013-07-03 周唐炬 创建</remarks>
    public class ParaInStockFilter
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int? SysNo { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        public int? ProductSysNo { get; set; }
        /// <summary>
        /// 实际入库数量
        /// </summary>
        public int? RealStockInQuantity { get; set; }
        /// <summary>
        /// 仓库编号
        /// </summary>
        public int? WarehouseSysNo { get; set; }
        /// <summary>
        /// 仓库编号集合
        /// </summary>
        public List<int> WarehouseSysNoList { get; set; }
        /// <summary>
        /// 来源单据类型：出库单(10)、RMA单(20)、借货单(30)
        /// </summary>
        public int? SourceType { get; set; }
        /// <summary>
        /// 来源单据编号
        /// </summary>
        public int? SourceSysNo { get; set; }
        /// <summary>
        /// 是否已打印
        /// </summary>
        public int? IsPrinted { get; set; }
        /// <summary>
        /// 状态：待入库（10）、部分入库（20）、已入库（50）、
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreatedDate { get; set; }
        /// <summary>
        /// 当前页号
        /// </summary>
        public int CurrentPage { get; set; }
    }
}
