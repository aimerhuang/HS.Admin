using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.ApiSupply.U1City
{
    public class Pro_Sku_Info
    {
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProTitle { get; set; }
        /// <summary>
        /// 商品货号
        /// </summary>
        public string ProNo { get; set; }
        /// <summary>
        /// 颜色名称
        /// </summary>
        public string ProColorName { get; set; }
        /// <summary>
        /// 规格名称
        /// </summary>
        public string ProSizesName { get; set; }
        /// <summary>
        /// sku码
        /// </summary>
        public string ProSkuNo { get; set; }
        /// <summary>
        /// 库存数量
        /// </summary>
        public int ProCount { get; set; }
    }
}
