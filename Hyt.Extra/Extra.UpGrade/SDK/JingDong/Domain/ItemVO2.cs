using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ItemVO2 : JdObject{


         [XmlElement("expandSortsList2")]
public  		List<string>
  expandSortsList2 { get; set; }


}
}
