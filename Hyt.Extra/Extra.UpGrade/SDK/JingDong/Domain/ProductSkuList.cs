using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ProductSkuList : JdObject{


         [XmlElement("resultCode")]
public  		int
  resultCode { get; set; }


         [XmlElement("skuSize")]
public  		List<string>
  skuSize { get; set; }


         [XmlElement("skuColor")]
public  		List<string>
  skuColor { get; set; }


}
}
