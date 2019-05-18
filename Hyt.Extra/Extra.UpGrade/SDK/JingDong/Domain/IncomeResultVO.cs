using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class IncomeResultVO : JdObject{


         [XmlElement("swiftNumber")]
public  		string
  swiftNumber { get; set; }


         [XmlElement("amount")]
public  		string
  amount { get; set; }


         [XmlElement("createTime")]
public  		string
  createTime { get; set; }


}
}
