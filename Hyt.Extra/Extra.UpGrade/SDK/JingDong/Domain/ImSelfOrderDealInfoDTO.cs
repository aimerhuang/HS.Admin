using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ImSelfOrderDealInfoDTO : JdObject{


         [XmlElement("orderNo")]
public  		string
  orderNo { get; set; }


         [XmlElement("returnCode")]
public  		string
  returnCode { get; set; }


}
}
