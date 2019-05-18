using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class SameTeam : JdObject{


         [XmlElement("beginTime")]
public  		long
  beginTime { get; set; }


         [XmlElement("endTime")]
public  		long
  endTime { get; set; }


         [XmlElement("groupId")]
public  		long
  groupId { get; set; }


         [XmlElement("reImage")]
public  		string
  reImage { get; set; }


         [XmlElement("nowNumber")]
public  		int
  nowNumber { get; set; }


         [XmlElement("teamId")]
public  		long
  teamId { get; set; }


         [XmlElement("teamPrice")]
public  		double
  teamPrice { get; set; }


         [XmlElement("teamType")]
public  		string
  teamType { get; set; }


         [XmlElement("title")]
public  		string
  title { get; set; }


}
}
