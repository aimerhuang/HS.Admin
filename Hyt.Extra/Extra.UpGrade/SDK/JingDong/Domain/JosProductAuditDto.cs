using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class JosProductAuditDto : JdObject{


         [XmlElement("applyId")]
public  		string
  applyId { get; set; }


         [XmlElement("auditorCode")]
public  		string
  auditorCode { get; set; }


         [XmlElement("auditorName")]
public  		string
  auditorName { get; set; }


         [XmlElement("opinion")]
public  		string
  opinion { get; set; }


         [XmlElement("operate_time")]
public  		DateTime
  operateTime { get; set; }


         [XmlElement("state")]
public  		int
  state { get; set; }


}
}
