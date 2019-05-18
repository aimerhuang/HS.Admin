using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class WarehouseDto : JdObject{


         [XmlElement("deliver_center_id")]
public  		string
  deliverCenterId { get; set; }


         [XmlElement("deliver_center_name")]
public  		string
  deliverCenterName { get; set; }


}
}
