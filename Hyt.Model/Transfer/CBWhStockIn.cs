using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 入库单扩展
    /// </summary>
    /// <remarks>2014-01-13 ZTJ 添加注释</remarks>
    public class CBWhStockIn : WhStockIn
    {
        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName { get; set; }

        /// <summary>
        /// 创建人名称
        /// </summary>
        public string CreatedByName { get; set; }

        /// <summary>
        /// 最后更新人名称
        /// </summary>
        public string LastUpdatedByName { get; set; }
    }
}
