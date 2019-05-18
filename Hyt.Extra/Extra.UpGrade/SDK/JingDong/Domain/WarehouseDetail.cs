using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class WarehouseDetail : JdObject{


         [XmlElement("warehouse_no")]
public  		string
  warehouseNo { get; set; }


         [XmlElement("warehouse_name")]
public  		string
  warehouseName { get; set; }


         [XmlElement("warehouse_address")]
public  		string
  warehouseAddress { get; set; }


         [XmlElement("warehouse_phone")]
public  		string
  warehousePhone { get; set; }


}
}
