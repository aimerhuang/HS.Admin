using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ConsumeResultVO : JdObject{


         [XmlElement("swiftNumber")]
public  		string
  swiftNumber { get; set; }


         [XmlElement("amount")]
public  		string
  amount { get; set; }


         [XmlElement("createTime")]
public  		string
  createTime { get; set; }


         [XmlElement("type")]
public  		string
  type { get; set; }


         [XmlElement("spaceId")]
public  		string
  spaceId { get; set; }


         [XmlElement("unitId")]
public  		string
  unitId { get; set; }


         [XmlElement("mediaId")]
public  		string
  mediaId { get; set; }


         [XmlElement("compaignId")]
public  		string
  compaignId { get; set; }


}
}
