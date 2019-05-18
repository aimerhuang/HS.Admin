using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ReturnOrderDetailForJosDto : JdObject{


         [XmlElement("skuId")]
public  		string
  skuId { get; set; }


         [XmlElement("commodityName")]
public  		string
  commodityName { get; set; }


         [XmlElement("commodityNum")]
public  		string
  commodityNum { get; set; }


}
}
