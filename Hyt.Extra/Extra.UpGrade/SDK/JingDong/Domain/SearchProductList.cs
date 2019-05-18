using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class SearchProductList : JdObject{


         [XmlElement("resultCode")]
public  		int
  resultCode { get; set; }


         [XmlElement("wareCount")]
public  		int
  wareCount { get; set; }


         [XmlElement("page")]
public  		int
  page { get; set; }


         [XmlElement("wareInfo")]
public  		List<string>
  wareInfo { get; set; }


}
}
