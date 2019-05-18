using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 配送单扩展
    /// </summary>
    /// <remarks>
    /// 2013/6/17 何方 创建
    /// </remarks>
    public partial class LgDelivery
    {
        /// <summary>
        /// 配送单明细
        /// </summary>
        /// <remarks>
        /// 2013-06-25 郑荣华 创建
        /// </remarks>
        public IList<LgDeliveryItem> LgDeliveryItemList { get; set; }
    }
}
