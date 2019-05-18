using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class SearchCatelogyList : JdObject{


         [XmlElement("resultCode")]
public  		int
  resultCode { get; set; }


         [XmlElement("wareCount")]
public  		int
  wareCount { get; set; }


         [XmlElement("page")]
public  		int
  page { get; set; }


         [XmlElement("regionIsTrue")]
public  		bool
  regionIsTrue { get; set; }


         [XmlElement("selfIsTrue")]
public  		bool
  selfIsTrue { get; set; }


         [XmlElement("show")]
public  		string
  show { get; set; }


         [XmlElement("wareInfo")]
public  		List<string>
  wareInfo { get; set; }


}
}
