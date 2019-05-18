using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class AutoAuditExport : JdObject{


         [XmlElement("serviceId")]
public  		int
  serviceId { get; set; }


         [XmlElement("vcCode")]
public  		string
  vcCode { get; set; }


}
}
