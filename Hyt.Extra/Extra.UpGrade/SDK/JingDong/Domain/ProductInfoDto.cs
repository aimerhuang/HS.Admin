using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ProductInfoDto : JdObject{


         [XmlElement("wareId")]
public  		string
  wareId { get; set; }


         [XmlElement("name")]
public  		string
  name { get; set; }


         [XmlElement("adWord")]
public  		string
  adWord { get; set; }


         [XmlElement("sortThird")]
public  		int
  sortThird { get; set; }


         [XmlElement("brandId")]
public  		int
  brandId { get; set; }


}
}
