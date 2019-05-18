using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Procurement
{
    public class CBWhWarehouseMonthStockHistory : WhWarehouseMonthStockHistory
    {
        public string ErpCode { get; set; }
        public string EasName { get; set; }
        public string Spec { get; set; }
        public string SpecUnit { get; set; }

        public int UpMonthQuantity { get; set; }
        public int UpMonthInQuantity { get; set; }
        public int UpMonthLossQuantity { get; set; }
        public int UpMonthSalesQuantity { get; set; }
        public int UpMonthRetQuantity { get; set; }
    }
    public class WarehouseMod
    {
        public int ProductSysNo { get; set; }
        public decimal TotalQuantity { get; set; }
    }
    /// <summary>
    /// 月份库存记录
    /// </summary>
    public class WhWarehouseMonthStockHistory
    {
        public int SysNo { get; set; }
        public int WarehouseSysNo { get; set; }
        public int ProductSysNo { get; set; }
        public int WhYear { get; set; }
        public int WhMonth { get; set; }
        public decimal InWareQuantity{get;set;}
        public decimal SalesQuantity { get; set; }
        public decimal RetQuantity { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal LossQuantity { get; set; }
    }
}
