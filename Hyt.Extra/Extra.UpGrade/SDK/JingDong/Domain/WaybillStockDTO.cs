using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class WaybillStockDTO : JdObject{


         [XmlElement("providerCode")]
public  		string
  providerCode { get; set; }


         [XmlElement("providerName")]
public  		string
  providerName { get; set; }


         [XmlElement("branchCode")]
public  		string
  branchCode { get; set; }


         [XmlElement("vendorCode")]
public  		string
  vendorCode { get; set; }


         [XmlElement("vendorName")]
public  		string
  vendorName { get; set; }


         [XmlElement("amount")]
public  		int
  amount { get; set; }


}
}
