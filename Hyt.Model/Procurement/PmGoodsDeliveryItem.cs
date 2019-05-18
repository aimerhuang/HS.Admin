using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Procurement
{
    public class CBPmGoodsDeliveryItem : PmGoodsDeliveryItem
    {
        public string Cb_ProName { get; set; }
        public string Cb_Unit { get; set; }
        public string Cb_Spec { get; set; }
        /// <summary>
        /// 订购数量
        /// </summary>
        public int Poi_ProQuity { get; set; }
    }
    public class PmGoodsDeliveryItem
    {
        public int SysNo { get; set; }
        public int gdi_PointItemSysNo { get; set; }
        public int gdi_GoodSysNo { get; set; }
        public int gdi_SendQuity { get; set; }
        public int gdi_PSysNo { get; set; }
    }
}
