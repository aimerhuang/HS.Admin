using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class PaginatedInfo : JdObject{


         [XmlElement("pageSize")]
public  		string
  pageSize { get; set; }


         [XmlElement("index")]
public  		string
  index { get; set; }


         [XmlElement("totalItem")]
public  		string
  totalItem { get; set; }


         [XmlElement("pageList")]
public  		List<string>
  pageList { get; set; }


         [XmlElement("totalPage")]
public  		string
  totalPage { get; set; }


}
}
