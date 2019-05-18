using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class Page : JdObject{


         [XmlElement("data")]
public  		List<string>
  data { get; set; }


         [XmlElement("pageNo")]
public  		int
  pageNo { get; set; }


         [XmlElement("pageSize")]
public  		int
  pageSize { get; set; }


         [XmlElement("totalItem")]
public  		long
  totalItem { get; set; }


}
}
