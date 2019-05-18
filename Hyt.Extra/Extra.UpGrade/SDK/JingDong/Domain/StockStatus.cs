using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class StockStatus : JdObject{


         [XmlElement("sku_id")]
public  		long
  skuId { get; set; }


         [XmlElement("area_id")]
public  		string
  areaId { get; set; }


         [XmlElement("stockState_id")]
public  		long
  stockStateId { get; set; }


         [XmlElement("stockState_desc")]
public  		string
  stockStateDesc { get; set; }


}
}
