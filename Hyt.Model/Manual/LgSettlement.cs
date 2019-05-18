using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 用于结算单明细视图
    /// </summary>
    /// <remarks>2013-07-04 黄伟 创建</remarks>
    public partial class LgSettlement
    {
        ///<summary>
        /// 结算单明细列表
        /// </summary>
        public IList<LgSettlementItem> Items { get; set; }
    }
}
