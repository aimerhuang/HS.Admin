using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class CancelOrder : JdObject{


         [XmlElement("orderNo")]
public  		long
  orderNo { get; set; }


         [XmlElement("cancelTime")]
public  		DateTime
  cancelTime { get; set; }


         [XmlElement("cancelReason")]
public  		string
  cancelReason { get; set; }


}
}
