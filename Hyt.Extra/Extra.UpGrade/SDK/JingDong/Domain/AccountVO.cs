using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class AccountVO : JdObject{


         [XmlElement("total_amount")]
public  		string
  totalAmount { get; set; }


         [XmlElement("available_amount")]
public  		string
  availableAmount { get; set; }


         [XmlElement("freeze_amount")]
public  		string
  freezeAmount { get; set; }


}
}
