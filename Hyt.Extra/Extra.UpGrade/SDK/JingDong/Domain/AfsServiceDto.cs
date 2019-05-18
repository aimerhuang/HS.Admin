using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class AfsServiceDto : JdObject{


         [XmlElement("customer_order_id")]
public  		string
  customerOrderId { get; set; }


         [XmlElement("customer_station_no")]
public  		string
  customerStationNo { get; set; }


         [XmlElement("customer_order_apply_time")]
public  		string
  customerOrderApplyTime { get; set; }


         [XmlElement("customer_order_verify_time")]
public  		string
  customerOrderVerifyTime { get; set; }


         [XmlElement("customer_order_finish_time")]
public  		string
  customerOrderFinishTime { get; set; }


         [XmlElement("customer_order_state")]
public  		string
  customerOrderState { get; set; }


         [XmlElement("customer_order_type")]
public  		string
  customerOrderType { get; set; }


         [XmlElement("customer_order_cert")]
public  		string
  customerOrderCert { get; set; }


         [XmlElement("customer_order_problem")]
public  		string
  customerOrderProblem { get; set; }


         [XmlElement("customer_order_return_way")]
public  		string
  customerOrderReturnWay { get; set; }


         [XmlElement("customer_order_contactor")]
public  		string
  customerOrderContactor { get; set; }


         [XmlElement("customer_order_contactor_tel")]
public  		string
  customerOrderContactorTel { get; set; }


         [XmlElement("customer_order_verify_remark")]
public  		string
  customerOrderVerifyRemark { get; set; }


         [XmlElement("order_id")]
public  		string
  orderId { get; set; }


         [XmlElement("vender_id")]
public  		string
  venderId { get; set; }


         [XmlElement("pay_type")]
public  		string
  payType { get; set; }


         [XmlElement("cash_refund")]
public  		string
  cashRefund { get; set; }


         [XmlElement("pos_refund")]
public  		string
  posRefund { get; set; }


         [XmlElement("pin_buyer")]
public  		string
  pinBuyer { get; set; }


         [XmlElement("customer_order_detail_list")]
public  		List<string>
  customerOrderDetailList { get; set; }


         [XmlElement("refund_list")]
public  		List<string>
  refundList { get; set; }


}
}
