using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 促销计算元数据
    /// </summary>
    /// <remarks>2014-03-11 吴文强 创建</remarks>
    public class SpPromotionToPython
    {
        #region 订单信息

        /// <summary>
        /// 是否是退换货计算
        /// true:是;false:否
        /// </summary>
        public bool IsReturn { get; set; }

        /// <summary>
        /// 订单信息
        /// </summary>
        public SoOrder Order { get; set; }

        #endregion
    }
}
