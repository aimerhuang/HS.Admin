using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class OuterOrderDetailDto : JdObject{


         [XmlElement("skuNo")]
public  		string
  skuNo { get; set; }


         [XmlElement("skuName")]
public  		string
  skuName { get; set; }


         [XmlElement("expectedQty")]
public  		string
  expectedQty { get; set; }


         [XmlElement("shippedQty")]
public  		string
  shippedQty { get; set; }


}
}
