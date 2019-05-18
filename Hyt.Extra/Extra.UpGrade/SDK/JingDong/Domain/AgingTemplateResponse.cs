using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class AgingTemplateResponse : JdObject{


         [XmlElement("resultCode")]
public  		int
  resultCode { get; set; }


         [XmlElement("resultMsg")]
public  		string
  resultMsg { get; set; }


         [XmlElement("agingTemplateList")]
public  		List<string>
  agingTemplateList { get; set; }


}
}
