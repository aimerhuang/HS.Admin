using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class QualificationRuleDto : JdObject{


         [XmlElement("type")]
public  		int
  type { get; set; }


         [XmlElement("qua_name")]
public  		string
  quaName { get; set; }


         [XmlElement("qua_required_rule")]
public  		string
  quaRequiredRule { get; set; }


}
}
