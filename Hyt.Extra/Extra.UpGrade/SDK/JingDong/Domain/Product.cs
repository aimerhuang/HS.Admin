using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class Product : JdObject{


         [XmlElement("skuId")]
public  		int
  skuId { get; set; }


         [XmlElement("skuPicUrl")]
public  		string
  skuPicUrl { get; set; }


         [XmlElement("skuName")]
public  		string
  skuName { get; set; }


}
}
