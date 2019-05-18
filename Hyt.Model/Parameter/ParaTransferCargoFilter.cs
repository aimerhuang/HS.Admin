using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 调货单筛选字段
    /// </summary>
    /// <remarks>2016-04-05 谭显锋 创建</remarks>
    public class ParaTransferCargoFilter
    {
        /// <summary>
        /// 出库单编号
        /// </summary>
        public int? SysNo { get; set; }

        /// <summary>
        /// 状态：(10 待处理  20 已确认 -10 已作废 -20 已打回)
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 申请仓库编号
        /// </summary>
        public int? WarehouseSysNo { get; set; }

        /// <summary>
        /// 配货仓库编号
        /// </summary>
        public int? DeliveryWarehouseSysNo { get; set; }

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
