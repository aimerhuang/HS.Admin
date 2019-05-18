using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 评价晒单销售单明细列表
    /// </summary>
    /// <remarks>
    /// 2013-08-15 杨晗 创建
    /// </remarks>
    public class CBFeCommentList
    {
        /// <summary>
        /// 销售单系统号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 销售单创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 产品系统号
        /// </summary>
        public int ProductSysNo { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 行号
        /// </summary>
        public int Rn { get; set; }
    }
}
