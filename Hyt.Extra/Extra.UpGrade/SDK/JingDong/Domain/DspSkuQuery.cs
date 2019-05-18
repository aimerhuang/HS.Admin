using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class DspSkuQuery : JdObject{


         [XmlElement("skuId")]
public  		long
  skuId { get; set; }


         [XmlElement("skuName")]
public  		string
  skuName { get; set; }


         [XmlElement("skuImg")]
public  		string
  skuImg { get; set; }


}
}
