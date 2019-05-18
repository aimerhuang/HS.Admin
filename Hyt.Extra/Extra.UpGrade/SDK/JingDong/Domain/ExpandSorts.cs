using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ExpandSorts : JdObject{


         [XmlElement("expandValueId")]
public  		int
  expandValueId { get; set; }


         [XmlElement("expandSortValueName")]
public  		string
  expandSortValueName { get; set; }


}
}
