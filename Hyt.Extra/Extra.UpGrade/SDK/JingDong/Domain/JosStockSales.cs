using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class JosStockSales : JdObject{


         [XmlElement("stock_count")]
public  		long
  stockCount { get; set; }


         [XmlElement("sales")]
public  		long
  sales { get; set; }


         [XmlElement("stock_turnover_days")]
public  		string
  stockTurnoverDays { get; set; }


         [XmlElement("stock_rate")]
public  		string
  stockRate { get; set; }


         [XmlElement("manual_vlt")]
public  		string
  manualVlt { get; set; }


         [XmlElement("auto_vlt")]
public  		string
  autoVlt { get; set; }


         [XmlElement("week")]
public  		string
  week { get; set; }


         [XmlElement("manual_sku_order_fill_rate")]
public  		string
  manualSkuOrderFillRate { get; set; }


         [XmlElement("manual_units_order_fill_rate")]
public  		string
  manualUnitsOrderFillRate { get; set; }


         [XmlElement("auto_sku_order_fill_rate")]
public  		string
  autoSkuOrderFillRate { get; set; }


         [XmlElement("auto_units_order_fill_rate")]
public  		string
  autoUnitsOrderFillRate { get; set; }


         [XmlElement("sku_order_fill_rate")]
public  		string
  skuOrderFillRate { get; set; }


         [XmlElement("units_order_fill_rate")]
public  		string
  unitsOrderFillRate { get; set; }


}
}
