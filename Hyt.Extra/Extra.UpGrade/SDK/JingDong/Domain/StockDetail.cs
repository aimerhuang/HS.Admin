using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class StockDetail : JdObject{


         [XmlElement("goods_no")]
public  		string
  goodsNo { get; set; }


         [XmlElement("warehouse_no")]
public  		string
  warehouseNo { get; set; }


         [XmlElement("stock_qty")]
public  		string
  stockQty { get; set; }


         [XmlElement("available_qty")]
public  		string
  availableQty { get; set; }


         [XmlElement("preemption_qty")]
public  		string
  preemptionQty { get; set; }


         [XmlElement("goods_status")]
public  		string
  goodsStatus { get; set; }


}
}
