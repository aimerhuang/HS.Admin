using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ProfitLossMDto : JdObject{


         [XmlElement("plNo")]
public  		string
  plNo { get; set; }


         [XmlElement("plType")]
public  		int
  plType { get; set; }


         [XmlElement("skuQty")]
public  		string
  skuQty { get; set; }


         [XmlElement("totalQty")]
public  		string
  totalQty { get; set; }


         [XmlElement("approveTime")]
public  		string
  approveTime { get; set; }


         [XmlElement("warehouseNo")]
public  		string
  warehouseNo { get; set; }


         [XmlElement("tenantId")]
public  		string
  tenantId { get; set; }


         [XmlElement("ownerNo")]
public  		string
  ownerNo { get; set; }


         [XmlElement("plDs")]
public  		List<string>
  plDs { get; set; }


}
}
