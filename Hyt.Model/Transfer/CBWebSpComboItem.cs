using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 商城前台组合套餐明细
    /// </summary>
    /// <remarks>2013-09-10 邵斌 创建</remarks>
    [Serializable]
    public class CBWebSpComboItem : SpComboItem
    {
        /// <summary>
        /// 商品促销主表系统编号
        /// </summary>
        public int PromotionSysNo { get; set; }

        /// <summary>
        /// 商品价格
        /// </summary>
        public IList<PdPrice> Prices { get; set; }

        /// <summary>
        /// 商品图片
        /// </summary>
        public string ProductImage { get; set; }
    }
}
