using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class PayInfo : JdObject{


         [XmlElement("payType")]
public  		int
  payType { get; set; }


         [XmlElement("payMoney")]
public  		long
  payMoney { get; set; }


}
}
