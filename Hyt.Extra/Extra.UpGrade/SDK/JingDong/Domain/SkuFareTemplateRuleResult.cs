using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class SkuFareTemplateRuleResult : JdObject{


         [XmlElement("resultStr")]
public  		string
  resultStr { get; set; }


         [XmlElement("types")]
public  		List<string>
  types { get; set; }


}
}
