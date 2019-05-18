using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class CrowdDmpVo : JdObject{


         [XmlElement("crowdId")]
public  		long
  crowdId { get; set; }


         [XmlElement("crowdName")]
public  		string
  crowdName { get; set; }


         [XmlElement("crowdType")]
public  		int
  crowdType { get; set; }


         [XmlElement("crowdTypeLable")]
public  		string
  crowdTypeLable { get; set; }


         [XmlElement("isUsed")]
public  		int
  isUsed { get; set; }


         [XmlElement("adGroupPrice")]
public  		int
  adGroupPrice { get; set; }


}
}
