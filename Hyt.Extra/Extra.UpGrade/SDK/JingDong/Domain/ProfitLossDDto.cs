using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ProfitLossDDto : JdObject{


         [XmlElement("skuNo")]
public  		string
  skuNo { get; set; }


         [XmlElement("skuName")]
public  		string
  skuName { get; set; }


         [XmlElement("plQty")]
public  		string
  plQty { get; set; }


         [XmlElement("productLevel")]
public  		string
  productLevel { get; set; }


}
}
