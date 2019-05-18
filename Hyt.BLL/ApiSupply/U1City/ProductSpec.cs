using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.ApiSupply.U1City
{
    [Serializable]
    public partial class ProductSpecs
    {
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
        /// SKU重量
        /// </summary>
        public decimal Weight { get; set; }
    }
}
