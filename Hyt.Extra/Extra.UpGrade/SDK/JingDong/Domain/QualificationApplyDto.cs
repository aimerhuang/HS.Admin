using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class QualificationApplyDto : JdObject{


         [XmlElement("apply_id")]
public  		string
  applyId { get; set; }


         [XmlElement("create_by")]
public  		string
  createBy { get; set; }


         [XmlElement("create_time")]
public  		DateTime
  createTime { get; set; }


         [XmlElement("modify_time")]
public  		DateTime
  modifyTime { get; set; }


         [XmlElement("state")]
public  		int
  state { get; set; }


         [XmlElement("yn")]
public  		int
  yn { get; set; }


         [XmlElement("qualification_info")]
public  		string
  qualificationInfo { get; set; }


         [XmlElement("item_audit_exts")]
public  		List<string>
  itemAuditExts { get; set; }


}
}
