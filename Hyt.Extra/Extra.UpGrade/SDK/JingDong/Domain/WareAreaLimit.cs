using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class WareAreaLimit : JdObject{


         [XmlElement("areaId")]
public  		long
  areaId { get; set; }


         [XmlElement("limitType")]
public  		int
  limitType { get; set; }


         [XmlElement("wareId")]
public  		long
  wareId { get; set; }


}
}
