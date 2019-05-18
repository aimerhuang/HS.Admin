using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class QueryStockInSamResult : JdObject{


         [XmlElement("code")]
public  		string
  code { get; set; }


         [XmlElement("msg")]
public  		string
  msg { get; set; }


         [XmlElement("list")]
public  		List<string>
  list { get; set; }


         [XmlElement("totalSize")]
public  		string
  totalSize { get; set; }


         [XmlElement("pageStart")]
public  		string
  pageStart { get; set; }


         [XmlElement("pageSize")]
public  		string
  pageSize { get; set; }


}
}
