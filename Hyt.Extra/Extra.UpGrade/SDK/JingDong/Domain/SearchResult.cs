using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class SearchResult : JdObject{


         [XmlElement("count")]
public  		int
  count { get; set; }


         [XmlElement("result")]
public  		List<string>
  result { get; set; }


}
}
