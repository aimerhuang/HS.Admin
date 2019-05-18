using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class AdwordResponse : JdObject{


         [XmlElement("skuId")]
public  		string
  skuId { get; set; }


         [XmlElement("promotionInfoList")]
public  		List<string>
  promotionInfoList { get; set; }


}
}
