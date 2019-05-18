using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ADAuditResult : JdObject{


         [XmlElement("auditInfo")]
public  		string
  auditInfo { get; set; }


}
}
