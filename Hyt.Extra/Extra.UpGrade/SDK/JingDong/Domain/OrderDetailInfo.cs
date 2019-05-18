using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class OrderDetailInfo : JdObject{


         [XmlElement("orderInfo")]
public  		OrderInfo 
  orderInfo { get; set; }


}
}
