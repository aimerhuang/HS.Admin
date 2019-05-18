using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ServiceProviderOrderListResult : JdObject{


         [XmlElement("header")]
public  		string
  header { get; set; }


         [XmlElement("body")]
public  		List<string>
  body { get; set; }


         [XmlElement("page")]
public  		string
  page { get; set; }


         [XmlElement("pageSize")]
public  		string
  pageSize { get; set; }


         [XmlElement("totalCount")]
public  		string
  totalCount { get; set; }


}
}
