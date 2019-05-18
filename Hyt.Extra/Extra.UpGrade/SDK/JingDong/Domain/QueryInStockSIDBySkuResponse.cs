using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class QueryInStockSIDBySkuResponse : JdObject{


         [XmlElement("total")]
public  		int
  total { get; set; }


         [XmlElement("pageSize")]
public  		int
  pageSize { get; set; }


         [XmlElement("pageNo")]
public  		int
  pageNo { get; set; }


         [XmlElement("serialNos")]
public  		List<string>
  serialNos { get; set; }


}
}
