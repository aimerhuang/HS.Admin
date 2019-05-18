using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 配送方式查询筛选字段
    /// </summary>
    /// <remarks>
    /// 2013-07-04 郑荣华 创建
    /// </remarks>
    public class ParaDeliveryTypeFilter
    {
        /// <summary>
        /// 配送方式名称
        /// </summary>
        public string DeliveryTypeName { get; set; }

        /// <summary>
        /// 前台是否可见：可见（1）、不可见（0）
        /// </summary>
        public int? IsOnlineVisible { get; set; }

        /// <summary>
        /// 状态：有效（1）、无效（0）
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 配送方式父级编号
        /// </summary>
        public int? ParentSysNo { get; set; }

        /// <summary>
        /// 需要过滤的配送方式系统编号
        /// 字符串 1,2,3,4
        /// </summary>
        public string SysNoFilter { get; set; }

        /// <summary>
        /// 仓库系统编号
        /// </summary>
        public int? WareHouseSysNo { get; set; }
    }
}
