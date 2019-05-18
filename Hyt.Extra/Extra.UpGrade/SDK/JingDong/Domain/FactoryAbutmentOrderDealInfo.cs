using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class FactoryAbutmentOrderDealInfo : JdObject{


         [XmlElement("orderno")]
public  		string
  orderno { get; set; }


         [XmlElement("returnCode")]
public  		string
  returnCode { get; set; }


}
}
