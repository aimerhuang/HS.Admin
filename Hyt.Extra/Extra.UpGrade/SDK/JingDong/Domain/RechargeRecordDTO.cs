using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class RechargeRecordDTO : JdObject{


         [XmlElement("providerCode")]
public  		string
  providerCode { get; set; }


         [XmlElement("providerName")]
public  		string
  providerName { get; set; }


         [XmlElement("branchCode")]
public  		string
  branchCode { get; set; }


         [XmlElement("branchName")]
public  		string
  branchName { get; set; }


         [XmlElement("state")]
public  		int
  state { get; set; }


         [XmlElement("operatorTime")]
public  		DateTime
  operatorTime { get; set; }


         [XmlElement("operatorName")]
public  		DateTime
  operatorName { get; set; }


         [XmlElement("amount")]
public  		int
  amount { get; set; }


}
}
