using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class CrmMemberResult : JdObject{


         [XmlElement("crm_members")]
public  		string
  crmMembers { get; set; }


         [XmlElement("total_result")]
public  		string
  totalResult { get; set; }


}
}
