using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class AdventGoodsStockResponse : JdObject{


         [XmlElement("deptNo")]
public  		string
  deptNo { get; set; }


         [XmlElement("warehouseNo")]
public  		string
  warehouseNo { get; set; }


         [XmlElement("details")]
public  		List<string>
  details { get; set; }


}
}
