using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class PurchaseOrderDto : JdObject{


         [XmlElement("order_id")]
public  		long
  orderId { get; set; }


         [XmlElement("created_date")]
public  		DateTime
  createdDate { get; set; }


         [XmlElement("provider_code")]
public  		string
  providerCode { get; set; }


         [XmlElement("provider_name")]
public  		string
  providerName { get; set; }


         [XmlElement("total_price")]
public  		string
  totalPrice { get; set; }


         [XmlElement("deliver_center_id")]
public  		int
  deliverCenterId { get; set; }


         [XmlElement("deliver_center_name")]
public  		string
  deliverCenterName { get; set; }


         [XmlElement("purchaser_name")]
public  		string
  purchaserName { get; set; }


         [XmlElement("purchaser_erp_code")]
public  		string
  purchaserErpCode { get; set; }


         [XmlElement("status")]
public  		int
  status { get; set; }


         [XmlElement("status_name")]
public  		string
  statusName { get; set; }


         [XmlElement("is_ept_customized")]
public  		bool
  isEptCustomized { get; set; }


         [XmlElement("state")]
public  		int
  state { get; set; }


         [XmlElement("state_name")]
public  		string
  stateName { get; set; }


         [XmlElement("complete_date")]
public  		DateTime
  completeDate { get; set; }


         [XmlElement("update_date")]
public  		DateTime
  updateDate { get; set; }


         [XmlElement("account_period")]
public  		int
  accountPeriod { get; set; }


         [XmlElement("receiver_name")]
public  		string
  receiverName { get; set; }


         [XmlElement("warehouse_phone")]
public  		string
  warehousePhone { get; set; }


         [XmlElement("address")]
public  		string
  address { get; set; }


         [XmlElement("order_type")]
public  		int
  orderType { get; set; }


         [XmlElement("order_type_name")]
public  		string
  orderTypeName { get; set; }


         [XmlElement("order_attribute")]
public  		int
  orderAttribute { get; set; }


         [XmlElement("order_attribute_name")]
public  		string
  orderAttributeName { get; set; }


         [XmlElement("confirm_state")]
public  		int
  confirmState { get; set; }


         [XmlElement("confirm_state_name")]
public  		string
  confirmStateName { get; set; }


         [XmlElement("custom_order_id")]
public  		long
  customOrderId { get; set; }


         [XmlElement("ware_variety")]
public  		int
  wareVariety { get; set; }


         [XmlElement("delivery_time")]
public  		DateTime
  deliveryTime { get; set; }


         [XmlElement("is_can_confirm")]
public  		bool
  isCanConfirm { get; set; }


         [XmlElement("is_exist_actual_num_dif")]
public  		int
  isExistActualNumDif { get; set; }


         [XmlElement("balance_status")]
public  		bool
  balanceStatus { get; set; }


         [XmlElement("storage_time")]
public  		DateTime
  storageTime { get; set; }


         [XmlElement("map")]
public  		string
  map { get; set; }


         [XmlElement("tc_flag")]
public  		int
  tcFlag { get; set; }


         [XmlElement("tc_flag_name")]
public  		string
  tcFlagName { get; set; }


         [XmlElement("book_time")]
public  		DateTime
  bookTime { get; set; }


}
}
