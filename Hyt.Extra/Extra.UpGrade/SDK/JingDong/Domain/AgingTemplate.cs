using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class AgingTemplate : JdObject{


         [XmlElement("templateId")]
public  		long
  templateId { get; set; }


         [XmlElement("templateName")]
public  		string
  templateName { get; set; }


}
}
