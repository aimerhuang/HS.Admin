using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class RequisitionInfoDetailDto : JdObject{


         [XmlElement("ware_name")]
public  		string
  wareName { get; set; }


         [XmlElement("price")]
public  		string
  price { get; set; }


         [XmlElement("warehouse_list")]
public  		List<string>
  warehouseList { get; set; }


         [XmlElement("reple_qty")]
public  		int
  repleQty { get; set; }


}
}
