using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 用于配送员路径查询使用配送单
    /// </summary>
    /// <remarks>
    /// 2013-07-03 郑荣华 创建
    /// </remarks>
    public class CBLgDeliveryInvoice
    {
        /// <summary>
        /// 配送单系统编号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 配送单创建时间
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// 配送单完成时间（即是结算单创建时间）
        /// </summary>
        public DateTime? EndDate { get; set; }

    }
}
