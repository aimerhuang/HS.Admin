using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class SkuSize : JdObject{


         [XmlElement("secKill")]
public  		bool
  secKill { get; set; }


         [XmlElement("description")]
public  		string
  description { get; set; }


         [XmlElement("promotion")]
public  		bool
  promotion { get; set; }


         [XmlElement("directShow")]
public  		bool
  directShow { get; set; }


         [XmlElement("canFreeRead")]
public  		bool
  canFreeRead { get; set; }


         [XmlElement("showMartPrice")]
public  		bool
  showMartPrice { get; set; }


         [XmlElement("skuId")]
public  		int
  skuId { get; set; }


         [XmlElement("size")]
public  		string
  size { get; set; }


}
}
