using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class SkuProperty : JdObject{


         [XmlElement("sku")]
public  		long
  sku { get; set; }


         [XmlElement("num")]
public  		int
  num { get; set; }


         [XmlElement("name")]
public  		string
  name { get; set; }


         [XmlElement("price")]
public  		double
  price { get; set; }


}
}
