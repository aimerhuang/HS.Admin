using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ReturnItem : JdObject{


         [XmlElement("return_item_id")]
public  		string
  returnItemId { get; set; }


         [XmlElement("attachment_code")]
public  		string
  attachmentCode { get; set; }


         [XmlElement("sku_id")]
public  		string
  skuId { get; set; }


         [XmlElement("sku_name")]
public  		string
  skuName { get; set; }


         [XmlElement("order_id")]
public  		string
  orderId { get; set; }


         [XmlElement("price")]
public  		string
  price { get; set; }


         [XmlElement("return_type")]
public  		string
  returnType { get; set; }


         [XmlElement("return_reason")]
public  		string
  returnReason { get; set; }


         [XmlElement("modifid_time")]
public  		string
  modifidTime { get; set; }


}
}
