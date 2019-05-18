using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 配送报表返回数据
    /// </summary>
 public   class CBPickingReportItem
 {
     public int ordersysno{get;set;}
     public int    shopno{get;set;}
     public string    shopname{get;set;}
     public string   name{get;set;}
     public string  streetaddress{get;set;}
     public string  mobilephonenumber{get;set;}
     public int areasysno{get;set;}
     public decimal  StockOutAmount{get;set;}
     public decimal Receivable{get;set;}
     public int ProductSysNo{get;set;}
     public string ProductName{get;set;}
     public string ProductQuantity{get;set;}
     public decimal OriginalPrice{get;set;}
     public decimal RealSalesAmount{get;set;}
     public string  erpcode{get;set;}
     public string mallorderid{get;set;}
     public int status{get;set;}
     public string areaallname { get; set; }
     public DateTime createddate { get; set; }
     public int stockoutno { get; set; }
     public string remarks { get; set; }
     public decimal freightamount { get; set; }
     public string expressno { get; set; }
     public string deliverytypename { get; set; }
    }
}
