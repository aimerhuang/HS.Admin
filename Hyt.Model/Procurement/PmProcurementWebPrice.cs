using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Procurement
{
    public class CBPmProcurementWebPrice : PmProcurementWebPrice
    {
        public string Pwt_Name { get; set; }
    }
    /// <summary>
    /// 采购申请单
    /// 2016-1-7 杨云奕 添加
    /// </summary>
    public class PmProcurementWebPrice
    {
        public int SysNo { get; set; }
        public int Pwp_TypeSysNo { get; set; }
        public int Pwp_ProSysNo { get; set; }
        public decimal Pwp_Price { get; set; }
        public int Pwp_OrderItemSysNo { get; set; }
    }
}
