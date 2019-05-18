using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class SkuClient : JdObject{


         [XmlElement("price")]
public  		string
  price { get; set; }


         [XmlElement("goodsType")]
public  		int
  goodsType { get; set; }


         [XmlElement("sku")]
public  		long
  sku { get; set; }


         [XmlElement("num")]
public  		int
  num { get; set; }


         [XmlElement("lowestReferencePrice")]
public  		string
  lowestReferencePrice { get; set; }


}
}
