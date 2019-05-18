using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class AccountBalanceVO : JdObject{


         [XmlElement("totalAmount")]
public  		string
  totalAmount { get; set; }


         [XmlElement("availableAmount")]
public  		string
  availableAmount { get; set; }


         [XmlElement("freezeAmount")]
public  		string
  freezeAmount { get; set; }


}
}
