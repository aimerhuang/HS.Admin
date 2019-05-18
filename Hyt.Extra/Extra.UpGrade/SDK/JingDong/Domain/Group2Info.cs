using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class Group2Info : JdObject{


         [XmlElement("group2_id")]
public  		string
  group2Id { get; set; }


         [XmlElement("group2_name")]
public  		string
  group2Name { get; set; }


         [XmlElement("group2_url")]
public  		string
  group2Url { get; set; }


}
}
