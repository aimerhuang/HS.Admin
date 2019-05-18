using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class OQueryResult : JdObject{


         [XmlElement("billType")]
public  		string
  billType { get; set; }


         [XmlElement("serialNumbers")]
public  		List<string>
  serialNumbers { get; set; }


         [XmlElement("storeId")]
public  		string
  storeId { get; set; }


         [XmlElement("storeName")]
public  		string
  storeName { get; set; }


         [XmlElement("totalRecords")]
public  		int
  totalRecords { get; set; }


         [XmlElement("pageSize")]
public  		int
  pageSize { get; set; }


         [XmlElement("pageNo")]
public  		int
  pageNo { get; set; }


}
}
