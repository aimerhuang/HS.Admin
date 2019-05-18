using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class JosApplyAuditDto : JdObject{


         [XmlElement("applyId")]
public  		string
  applyId { get; set; }


         [XmlElement("opinion")]
public  		string
  opinion { get; set; }


         [XmlElement("operateTime")]
public  		DateTime
  operateTime { get; set; }


         [XmlElement("erpCode")]
public  		string
  erpCode { get; set; }


         [XmlElement("state")]
public  		string
  state { get; set; }


}
}
