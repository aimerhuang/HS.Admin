using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class QueryOrderDetailListJos : JdObject{


         [XmlElement("details")]
public  		List<string>
  details { get; set; }


}
}
