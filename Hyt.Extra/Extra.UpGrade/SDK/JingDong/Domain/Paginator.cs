using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class Paginator : JdObject{


         [XmlElement("page")]
public  		int
  page { get; set; }


         [XmlElement("items")]
public  		int
  items { get; set; }


         [XmlElement("itemsPerPage")]
public  		int
  itemsPerPage { get; set; }


}
}
