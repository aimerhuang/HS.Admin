using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Procurement
{
    public class CBPmPointsOrderItem : PmPointsOrderItem
    {
        public string Cb_ProName { get; set; }
        public string Cb_Unit { get; set; }
        public string Cb_Spec { get; set; }


        public int Poi_ProSysNo { get; set; }
        public string Poi_ProName { get; set; }
        public decimal Poi_TradePrice { get; set; }
        public decimal Poi_JoinPrice { get; set; }
        public decimal Poi_SalePrice { get; set; }
        public int Poi_ProQuity { get; set; }


        public List<PmProcurementWebType> WebTypeList = new List<PmProcurementWebType>();
    }
    public class PmPointsOrderItem
    {
        public int SysNo { get; set; }
        public int Poi_PSysNo { get; set; }
        public int Poi_ProcurementItemSysNo { get; set; }

        public int Poi_Status { get; set; }
    }
}
