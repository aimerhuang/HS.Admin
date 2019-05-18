using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 取件单查询条件筛选实体
    /// </summary>
    /// <remarks>
    /// 2013-07-04 郑荣华 创建
    /// </remarks>
    public class ParaPickUpFilter
    {

        /// <summary>
        /// 仓库编号
        /// </summary>
        public int? WarehouseSysNo { get; set; }

        /// <summary>
        /// 状态：待取件（10）、已取件（20）、已入库（30）、作
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 需要过滤的取件单(1,2,3,4,5,6)
        /// </summary>
        public string SysNoFilter { get; set; }

        /// <summary>
        /// 取件单系统编号或入库单系统编号
        /// </summary>
        public int? SysNo { get; set; }

        /// <summary>
        /// 取件方式系统编号
        /// </summary>
        public int? PickupTypeSysNo { get; set; }

        /// <summary>
        /// 仓库编号集合
        /// </summary>
        public List<int> WarehouseSysNoList { get; set; }

        /// <summary>
        /// 当前页
        /// </summary>
        public int CurrentPage { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; set; }
    }
}
