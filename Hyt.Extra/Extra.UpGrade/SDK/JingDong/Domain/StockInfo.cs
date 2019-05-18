using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class StockInfo : JdObject{


         [XmlElement("sku")]
public  		string
  sku { get; set; }


         [XmlElement("skuNum")]
public  		string
  skuNum { get; set; }


         [XmlElement("barcode")]
public  		string
  barcode { get; set; }


         [XmlElement("serialNos")]
public  		string
  serialNos { get; set; }


}
}
