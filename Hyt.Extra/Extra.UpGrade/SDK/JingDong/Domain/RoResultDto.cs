using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class RoResultDto : JdObject{


         [XmlElement("pageIndex")]
public  		int
  pageIndex { get; set; }


         [XmlElement("pageSize")]
public  		int
  pageSize { get; set; }


         [XmlElement("recordCount")]
public  		int
  recordCount { get; set; }


         [XmlElement("roDtoList")]
public  		List<string>
  roDtoList { get; set; }


}
}
