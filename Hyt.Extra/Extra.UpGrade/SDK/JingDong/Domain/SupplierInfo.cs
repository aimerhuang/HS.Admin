using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class SupplierInfo : JdObject{


         [XmlElement("supplierNo")]
public  		string
  supplierNo { get; set; }


         [XmlElement("supplierName")]
public  		string
  supplierName { get; set; }


         [XmlElement("contact")]
public  		string
  contact { get; set; }


         [XmlElement("mobile")]
public  		string
  mobile { get; set; }


         [XmlElement("email")]
public  		string
  email { get; set; }


         [XmlElement("addr")]
public  		string
  addr { get; set; }


         [XmlElement("rtnAddr")]
public  		string
  rtnAddr { get; set; }


         [XmlElement("memo")]
public  		string
  memo { get; set; }


}
}
