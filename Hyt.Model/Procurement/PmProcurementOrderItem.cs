using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Procurement
{
    /// <summary>
    /// 采购申请单
    /// 2016-1-7 杨云奕 添加
    /// </summary>
    public class PmProcurementOrderItem
    {
        public int SysNo { get; set; }
        public int Poi_PSysNo { get; set; }
        public int Poi_ProSysNo { get; set; }
        public string Poi_ProName { get; set; }
        public decimal Poi_TradePrice { get; set; }
        public decimal Poi_JoinPrice { get; set; }
        public decimal Poi_SalePrice { get; set; }
        public int Poi_ProQuity { get; set; }
        public string Poi_DisInfo { get; set; }
        public int Poi_Status { get; set; }
    }
}
