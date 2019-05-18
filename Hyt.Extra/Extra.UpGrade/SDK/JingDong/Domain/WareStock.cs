using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class WareStock : JdObject{


         [XmlElement("sku")]
public  		long
  sku { get; set; }


         [XmlElement("hasStock")]
public  		bool
  hasStock { get; set; }


         [XmlElement("remainNum")]
public  		int
  remainNum { get; set; }


}
}
