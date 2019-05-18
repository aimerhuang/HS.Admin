using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class OrderHistoryResponse : JdObject{


         [XmlElement("totalCount")]
public  		int
  totalCount { get; set; }


         [XmlElement("pageSize")]
public  		int
  pageSize { get; set; }


         [XmlElement("pageNo")]
public  		int
  pageNo { get; set; }


         [XmlElement("orders")]
public  		List<string>
  orders { get; set; }


         [XmlElement("resultCode")]
public  		int
  resultCode { get; set; }


         [XmlElement("message")]
public  		string
  message { get; set; }


}
}
