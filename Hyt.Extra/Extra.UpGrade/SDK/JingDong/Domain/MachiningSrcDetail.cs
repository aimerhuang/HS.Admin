using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class MachiningSrcDetail : JdObject{


         [XmlElement("ownerNo")]
public  		string
  ownerNo { get; set; }


         [XmlElement("skuNo")]
public  		string
  skuNo { get; set; }


         [XmlElement("productLevel")]
public  		string
  productLevel { get; set; }


         [XmlElement("qty")]
public  		string
  qty { get; set; }


}
}
