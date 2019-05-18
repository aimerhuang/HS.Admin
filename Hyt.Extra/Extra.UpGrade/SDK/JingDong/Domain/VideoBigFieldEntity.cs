using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class VideoBigFieldEntity : JdObject{


         [XmlElement("skuId")]
public  		int
  skuId { get; set; }


         [XmlElement("firstClassId")]
public  		int
  firstClassId { get; set; }


         [XmlElement("videoBigFieldInfo")]
public  		string
  videoBigFieldInfo { get; set; }


}
}
