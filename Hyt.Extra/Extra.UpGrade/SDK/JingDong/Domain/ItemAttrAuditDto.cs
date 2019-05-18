using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ItemAttrAuditDto : JdObject{


         [XmlElement("operate_time")]
public  		DateTime
  operateTime { get; set; }


         [XmlElement("erp_code")]
public  		string
  erpCode { get; set; }


         [XmlElement("opinion")]
public  		string
  opinion { get; set; }


         [XmlElement("erp_name")]
public  		string
  erpName { get; set; }


         [XmlElement("audit_state")]
public  		int
  auditState { get; set; }


}
}
