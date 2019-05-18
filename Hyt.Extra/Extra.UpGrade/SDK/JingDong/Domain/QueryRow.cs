using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class QueryRow : JdObject{


         [XmlElement("values")]
public  		List<string>
  values { get; set; }


}
}
