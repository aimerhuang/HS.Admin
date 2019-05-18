using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class OrderinfoResponse : JdObject{


         [XmlElement("order_id")]
public  		string
  orderId { get; set; }


         [XmlElement("vender_id")]
public  		string
  venderId { get; set; }


         [XmlElement("pay_type")]
public  		string
  payType { get; set; }


         [XmlElement("sorting_type")]
public  		int
  sortingType { get; set; }


         [XmlElement("order_stock_owner")]
public  		string
  orderStockOwner { get; set; }


         [XmlElement("order_total_price")]
public  		double
  orderTotalPrice { get; set; }


         [XmlElement("order_payment")]
public  		double
  orderPayment { get; set; }


         [XmlElement("order_seller_price")]
public  		double
  orderSellerPrice { get; set; }


         [XmlElement("freight_price")]
public  		double
  freightPrice { get; set; }


         [XmlElement("seller_discount")]
public  		double
  sellerDiscount { get; set; }


         [XmlElement("pin_buyer")]
public  		string
  pinBuyer { get; set; }


         [XmlElement("delivery_type")]
public  		string
  deliveryType { get; set; }


         [XmlElement("order_type")]
public  		string
  orderType { get; set; }


         [XmlElement("invoice_state")]
public  		string
  invoiceState { get; set; }


         [XmlElement("invoice_info")]
public  		string
  invoiceInfo { get; set; }


         [XmlElement("order_remark")]
public  		string
  orderRemark { get; set; }


         [XmlElement("order_start_time")]
public  		DateTime
  orderStartTime { get; set; }


         [XmlElement("order_end_time")]
public  		DateTime
  orderEndTime { get; set; }


         [XmlElement("full_name")]
public  		string
  fullName { get; set; }


         [XmlElement("full_address")]
public  		string
  fullAddress { get; set; }


         [XmlElement("telephone")]
public  		string
  telephone { get; set; }


         [XmlElement("mobile")]
public  		string
  mobile { get; set; }


         [XmlElement("province")]
public  		int
  province { get; set; }


         [XmlElement("city")]
public  		int
  city { get; set; }


         [XmlElement("county")]
public  		int
  county { get; set; }


         [XmlElement("town")]
public  		int
  town { get; set; }


         [XmlElement("province_name")]
public  		string
  provinceName { get; set; }


         [XmlElement("city_name")]
public  		string
  cityName { get; set; }


         [XmlElement("county_name")]
public  		string
  countyName { get; set; }


         [XmlElement("town_name")]
public  		string
  townName { get; set; }


         [XmlElement("item_info_list")]
public  		List<string>
  itemInfoList { get; set; }


         [XmlElement("coupon_detail_list")]
public  		List<string>
  couponDetailList { get; set; }


         [XmlElement("order_state_list")]
public  		List<string>
  orderStateList { get; set; }


         [XmlElement("return_order")]
public  		string
  returnOrder { get; set; }


         [XmlElement("vender_remark")]
public  		string
  venderRemark { get; set; }


         [XmlElement("modified")]
public  		DateTime
  modified { get; set; }


         [XmlElement("station_order_state")]
public  		string
  stationOrderState { get; set; }


         [XmlElement("station_order_update_time")]
public  		DateTime
  stationOrderUpdateTime { get; set; }


         [XmlElement("station_no")]
public  		string
  stationNo { get; set; }


         [XmlElement("station_no_isv")]
public  		string
  stationNoIsv { get; set; }


         [XmlElement("station_name")]
public  		string
  stationName { get; set; }


         [XmlElement("station_order_type")]
public  		string
  stationOrderType { get; set; }


         [XmlElement("order_cancel_time")]
public  		DateTime
  orderCancelTime { get; set; }


         [XmlElement("order_cancel_reason")]
public  		string
  orderCancelReason { get; set; }


         [XmlElement("order_backward_remark")]
public  		string
  orderBackwardRemark { get; set; }


         [XmlElement("station_payment_type")]
public  		string
  stationPaymentType { get; set; }


         [XmlElement("station_payment_cash")]
public  		double
  stationPaymentCash { get; set; }


         [XmlElement("station_payment_pos")]
public  		double
  stationPaymentPos { get; set; }


         [XmlElement("station_delivery_type")]
public  		string
  stationDeliveryType { get; set; }


         [XmlElement("carrier_no")]
public  		string
  carrierNo { get; set; }


         [XmlElement("carrier_name")]
public  		string
  carrierName { get; set; }


         [XmlElement("deliver_no")]
public  		string
  deliverNo { get; set; }


}
}
