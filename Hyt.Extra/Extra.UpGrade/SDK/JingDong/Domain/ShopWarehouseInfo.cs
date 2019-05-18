using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ShopWarehouseInfo : JdObject{


         [XmlElement("deptNo")]
public  		string
  deptNo { get; set; }


         [XmlElement("shopNos")]
public  		string
  shopNos { get; set; }


         [XmlElement("warehouseNos")]
public  		string
  warehouseNos { get; set; }


}
}
