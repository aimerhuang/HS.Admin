using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 借货单
    /// </summary>
    /// <remarks>2013-07-13 周唐炬 创建</remarks>
    public partial class WhProductLend : BaseEntity
    {
        /// <summary>
        /// 借货单商品明细
        /// </summary>
        public List<WhProductLendItem> ItemList { get; set; }
    }
}
