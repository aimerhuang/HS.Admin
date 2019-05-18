using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class DeptOut : JdObject{


         [XmlElement("deptNo")]
public  		string
  deptNo { get; set; }


         [XmlElement("deptName")]
public  		string
  deptName { get; set; }


         [XmlElement("sellerNo")]
public  		string
  sellerNo { get; set; }


         [XmlElement("sellerName")]
public  		string
  sellerName { get; set; }


         [XmlElement("enableTemplate")]
public  		string
  enableTemplate { get; set; }


         [XmlElement("managerName")]
public  		string
  managerName { get; set; }


         [XmlElement("managerPhone")]
public  		string
  managerPhone { get; set; }


         [XmlElement("managerFax")]
public  		string
  managerFax { get; set; }


         [XmlElement("managerEmail")]
public  		string
  managerEmail { get; set; }


         [XmlElement("managerAddress")]
public  		string
  managerAddress { get; set; }


         [XmlElement("settlementMode")]
public  		string
  settlementMode { get; set; }


         [XmlElement("settlementBody")]
public  		string
  settlementBody { get; set; }


         [XmlElement("resultsSection")]
public  		string
  resultsSection { get; set; }


         [XmlElement("accountData")]
public  		string
  accountData { get; set; }


         [XmlElement("qualification")]
public  		string
  qualification { get; set; }


         [XmlElement("billingConditions")]
public  		string
  billingConditions { get; set; }


         [XmlElement("status")]
public  		string
  status { get; set; }


         [XmlElement("reserve1")]
public  		string
  reserve1 { get; set; }


         [XmlElement("reserve2")]
public  		string
  reserve2 { get; set; }


         [XmlElement("reserve3")]
public  		string
  reserve3 { get; set; }


         [XmlElement("reserve4")]
public  		string
  reserve4 { get; set; }


         [XmlElement("reserve5")]
public  		string
  reserve5 { get; set; }


}
}
