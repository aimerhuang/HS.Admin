using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class StockDto : JdObject{


         [XmlElement("skuNo")]
public  		string
  skuNo { get; set; }


         [XmlElement("skuName")]
public  		string
  skuName { get; set; }


         [XmlElement("qty")]
public  		string
  qty { get; set; }


         [XmlElement("canLocateQty")]
public  		string
  canLocateQty { get; set; }


         [XmlElement("ownerNo")]
public  		string
  ownerNo { get; set; }


         [XmlElement("StorageId")]
public  		string
  StorageId { get; set; }


         [XmlElement("productLevel")]
public  		string
  productLevel { get; set; }


         [XmlElement("productLevelName")]
public  		string
  productLevelName { get; set; }


         [XmlElement("warehouseNo")]
public  		string
  warehouseNo { get; set; }


         [XmlElement("tenantId")]
public  		string
  tenantId { get; set; }


}
}
