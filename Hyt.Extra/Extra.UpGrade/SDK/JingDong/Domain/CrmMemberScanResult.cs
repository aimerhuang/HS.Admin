using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class CrmMemberScanResult : JdObject{


         [XmlElement("crm_members")]
public  		string
  crmMembers { get; set; }


         [XmlElement("total_result")]
public  		string
  totalResult { get; set; }


         [XmlElement("scroll_id")]
public  		string
  scrollId { get; set; }


}
}
