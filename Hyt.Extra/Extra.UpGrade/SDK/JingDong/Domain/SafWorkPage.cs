using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class SafWorkPage : JdObject{


         [XmlElement("result")]
public  		List<string>
  result { get; set; }


         [XmlElement("total_count")]
public  		string
  totalCount { get; set; }


}
}
