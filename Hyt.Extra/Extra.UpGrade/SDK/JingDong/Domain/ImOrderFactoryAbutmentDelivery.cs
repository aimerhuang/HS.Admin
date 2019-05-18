using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ImOrderFactoryAbutmentDelivery : JdObject{


         [XmlElement("orderNo")]
public  		string
  orderNo { get; set; }


         [XmlElement("deliveryTime")]
public  		DateTime
  deliveryTime { get; set; }


}
}
