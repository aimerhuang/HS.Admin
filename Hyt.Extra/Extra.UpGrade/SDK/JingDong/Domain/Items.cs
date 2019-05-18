using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class Items : JdObject{


         [XmlElement("items")]
public  		List<string>
  items { get; set; }


         [XmlElement("items2")]
public  		List<string>
  items2 { get; set; }


         [XmlElement("expandSortValueName")]
public  		string
  expandSortValueName { get; set; }


}
}
