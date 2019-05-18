using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class StateQueryResultVO : JdObject{


         [XmlElement("sku")]
public  		string
  sku { get; set; }


         [XmlElement("a")]
public  		string
  a { get; set; }


}
}
