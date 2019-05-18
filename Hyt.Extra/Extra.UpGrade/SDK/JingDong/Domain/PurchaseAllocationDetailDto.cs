using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class PurchaseAllocationDetailDto : JdObject{


         [XmlElement("order_id")]
public  		long
  orderId { get; set; }


         [XmlElement("ware_id")]
public  		long
  wareId { get; set; }


         [XmlElement("deliver_center_id")]
public  		int
  deliverCenterId { get; set; }


         [XmlElement("deliver_center_name")]
public  		string
  deliverCenterName { get; set; }


         [XmlElement("ware_name")]
public  		string
  wareName { get; set; }


         [XmlElement("purchase_price")]
public  		string
  purchasePrice { get; set; }


         [XmlElement("original_num")]
public  		int
  originalNum { get; set; }


         [XmlElement("confirm_num")]
public  		int
  confirmNum { get; set; }


         [XmlElement("actual_num")]
public  		int
  actualNum { get; set; }


         [XmlElement("non_delivery_reason")]
public  		string
  nonDeliveryReason { get; set; }


         [XmlElement("back_explanation_type")]
public  		int
  backExplanationType { get; set; }


         [XmlElement("totoal_price")]
public  		string
  totoalPrice { get; set; }


         [XmlElement("remark")]
public  		string
  remark { get; set; }


         [XmlElement("isbn")]
public  		string
  isbn { get; set; }


         [XmlElement("make_price")]
public  		string
  makePrice { get; set; }


         [XmlElement("current_make_price")]
public  		string
  currentMakePrice { get; set; }


         [XmlElement("discount")]
public  		string
  discount { get; set; }


         [XmlElement("store_id")]
public  		int
  storeId { get; set; }


         [XmlElement("store_name")]
public  		string
  storeName { get; set; }


         [XmlElement("purchase_ware_property")]
public  		string
  purchaseWareProperty { get; set; }


}
}
