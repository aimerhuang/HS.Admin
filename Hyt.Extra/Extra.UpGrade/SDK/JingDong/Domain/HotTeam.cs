using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class HotTeam : JdObject{


         [XmlElement("teamId")]
public  		long
  teamId { get; set; }


         [XmlElement("groupId")]
public  		long
  groupId { get; set; }


         [XmlElement("title")]
public  		string
  title { get; set; }


         [XmlElement("teamPrice")]
public  		double
  teamPrice { get; set; }


         [XmlElement("beginTime")]
public  		long
  beginTime { get; set; }


         [XmlElement("endTime")]
public  		long
  endTime { get; set; }


         [XmlElement("nowNumber")]
public  		int
  nowNumber { get; set; }


         [XmlElement("image")]
public  		string
  image { get; set; }


}
}
