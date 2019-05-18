using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ServiceCustomerInfoExport : JdObject{


         [XmlElement("customerPin")]
public  		string
  customerPin { get; set; }


         [XmlElement("customerName")]
public  		string
  customerName { get; set; }


         [XmlElement("customerContactName")]
public  		string
  customerContactName { get; set; }


         [XmlElement("customerTel")]
public  		string
  customerTel { get; set; }


         [XmlElement("customerMobilePhone")]
public  		string
  customerMobilePhone { get; set; }


         [XmlElement("customerEmail")]
public  		string
  customerEmail { get; set; }


         [XmlElement("customerPostcode")]
public  		string
  customerPostcode { get; set; }


         [XmlElement("customerGrade")]
public  		int
  customerGrade { get; set; }


}
}
