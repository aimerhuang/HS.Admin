using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Common
{
    /// <summary>
    /// 2016-04-09 杨云奕 添加
    /// 用于统计报表实体
    /// </summary>
    public class ReportMod
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// 描述内容
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public decimal TotalAmount { get; set; }
    }
}
