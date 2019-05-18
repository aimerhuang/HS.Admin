using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class SerialTracDetail : JdObject{


         [XmlElement("tenantId")]
public  		string
  tenantId { get; set; }


         [XmlElement("warehouseNo")]
public  		string
  warehouseNo { get; set; }


         [XmlElement("billType")]
public  		string
  billType { get; set; }


         [XmlElement("orderNo")]
public  		string
  orderNo { get; set; }


         [XmlElement("skuNo")]
public  		string
  skuNo { get; set; }


         [XmlElement("serialNo")]
public  		string
  serialNo { get; set; }


         [XmlElement("id")]
public  		int
  id { get; set; }


         [XmlElement("ownerNo")]
public  		string
  ownerNo { get; set; }


         [XmlElement("createTime")]
public  		DateTime
  createTime { get; set; }


}
}
