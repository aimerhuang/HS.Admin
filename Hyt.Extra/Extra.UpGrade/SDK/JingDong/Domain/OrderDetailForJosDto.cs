using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class OrderDetailForJosDto : JdObject{


         [XmlElement("skuId")]
public  		string
  skuId { get; set; }


         [XmlElement("commodityName")]
public  		string
  commodityName { get; set; }


         [XmlElement("commodityNum")]
public  		int
  commodityNum { get; set; }


         [XmlElement("jdPrice")]
public  		string
  jdPrice { get; set; }


         [XmlElement("discount")]
public  		string
  discount { get; set; }


         [XmlElement("cost")]
public  		string
  cost { get; set; }


}
}
