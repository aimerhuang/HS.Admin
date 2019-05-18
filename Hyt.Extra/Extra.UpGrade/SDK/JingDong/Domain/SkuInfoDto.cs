using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class SkuInfoDto : JdObject{


         [XmlElement("skuId")]
public  		string
  skuId { get; set; }


         [XmlElement("skuName")]
public  		string
  skuName { get; set; }


         [XmlElement("uuid")]
public  		string
  uuid { get; set; }


         [XmlElement("color")]
public  		string
  color { get; set; }


         [XmlElement("colorSort")]
public  		int
  colorSort { get; set; }


         [XmlElement("size")]
public  		string
  size { get; set; }


         [XmlElement("sizeSort")]
public  		int
  sizeSort { get; set; }


         [XmlElement("market_price")]
public  		string
  marketPrice { get; set; }


         [XmlElement("purchase_price")]
public  		string
  purchasePrice { get; set; }


         [XmlElement("member_price")]
public  		string
  memberPrice { get; set; }


         [XmlElement("weight")]
public  		string
  weight { get; set; }


         [XmlElement("length")]
public  		int
  length { get; set; }


         [XmlElement("width")]
public  		int
  width { get; set; }


         [XmlElement("height")]
public  		int
  height { get; set; }


         [XmlElement("upc")]
public  		string
  upc { get; set; }


         [XmlElement("itemNum")]
public  		string
  itemNum { get; set; }


}
}
