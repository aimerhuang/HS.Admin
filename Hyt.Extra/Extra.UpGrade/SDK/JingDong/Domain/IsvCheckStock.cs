using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class IsvCheckStock : JdObject{


         [XmlElement("checkStockNo")]
public  		string
  checkStockNo { get; set; }


         [XmlElement("warehouseId")]
public  		string
  warehouseId { get; set; }


         [XmlElement("createTime")]
public  		DateTime
  createTime { get; set; }


         [XmlElement("deptNo")]
public  		string
  deptNo { get; set; }


         [XmlElement("details")]
public  		List<string>
  details { get; set; }


}
}
