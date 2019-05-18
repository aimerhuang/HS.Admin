using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class JosReturnOrderLineDTO : JdObject{


         [XmlElement("returnOrderCode")]
public  		string
  returnOrderCode { get; set; }


         [XmlElement("vendorProductId")]
public  		string
  vendorProductId { get; set; }


         [XmlElement("jdSku")]
public  		string
  jdSku { get; set; }


         [XmlElement("productName")]
public  		string
  productName { get; set; }


         [XmlElement("productCode")]
public  		string
  productCode { get; set; }


         [XmlElement("quantity")]
public  		int
  quantity { get; set; }


         [XmlElement("pricing")]
public  		string
  pricing { get; set; }


         [XmlElement("salesPrice")]
public  		string
  salesPrice { get; set; }


         [XmlElement("sparePartReturnPrice")]
public  		string
  sparePartReturnPrice { get; set; }


         [XmlElement("discountRate")]
public  		string
  discountRate { get; set; }


         [XmlElement("returnBased")]
public  		string
  returnBased { get; set; }


         [XmlElement("receivingQty")]
public  		int
  receivingQty { get; set; }


         [XmlElement("damagedQty")]
public  		int
  damagedQty { get; set; }


         [XmlElement("remark")]
public  		string
  remark { get; set; }


         [XmlElement("idPart")]
public  		string
  idPart { get; set; }


         [XmlElement("damageReason")]
public  		string
  damageReason { get; set; }


         [XmlElement("createTime")]
public  		DateTime
  createTime { get; set; }


         [XmlElement("updateTime")]
public  		DateTime
  updateTime { get; set; }


}
}
