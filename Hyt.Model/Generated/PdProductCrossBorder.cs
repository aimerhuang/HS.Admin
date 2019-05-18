using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Generated
{
    public class PdProductCrossBorder
    {
        public int SysNo { get; set; }
        public int ProductSysNo { get; set; }
        /// <summary>
        /// 商检商品备案号
        /// </summary>
        public string CIQGoodsNo { get; set; }
        /// <summary>
        /// 海关商品备案号
        /// </summary>
        public string CusGoodsNo { get; set; }
        /// <summary>
        /// JGS-20 海关业务代码 中的单位代码
        /// </summary>
        public string Unit { get; set; }
    }
}
