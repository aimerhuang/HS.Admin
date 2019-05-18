using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class CombineBuyingWidsList : JdObject{


         [XmlElement("resultCode")]
public  		int
  resultCode { get; set; }


         [XmlElement("combineWareId")]
public  		string
  combineWareId { get; set; }


         [XmlElement("showClick")]
public  		string
  showClick { get; set; }


         [XmlElement("combineBuyingWids")]
public  		List<string>
  combineBuyingWids { get; set; }


}
}
