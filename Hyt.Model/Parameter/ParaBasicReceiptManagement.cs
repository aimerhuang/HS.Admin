using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 科目代码查询
    /// </summary>
    /// <remarks>2014-1-15 沈强 创建</remarks>
    public class ParaBasicReceiptManagement:FnReceiptTitleAssociation
    {
        /// <summary>
        /// 科目代码/名称
        /// </summary>
        public string CodeOrName { get; set; }

        /// <summary>
        /// 是否默认收款科目
        /// </summary>
        public int? IsDef { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public int? PayMentType { get; set; }
    }
}
