using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class QueryOrderDetailJos : JdObject{


         [XmlElement("orderId")]
public  		string
  orderId { get; set; }


         [XmlElement("orderType")]
public  		string
  orderType { get; set; }


         [XmlElement("orderStatus")]
public  		string
  orderStatus { get; set; }


         [XmlElement("payStatus")]
public  		string
  payStatus { get; set; }


         [XmlElement("locked")]
public  		int
  locked { get; set; }


         [XmlElement("disputed")]
public  		int
  disputed { get; set; }


         [XmlElement("userPin")]
public  		string
  userPin { get; set; }


         [XmlElement("consignee")]
public  		string
  consignee { get; set; }


         [XmlElement("consigneeAddr")]
public  		string
  consigneeAddr { get; set; }


         [XmlElement("expressCorp")]
public  		string
  expressCorp { get; set; }


         [XmlElement("expressNo")]
public  		string
  expressNo { get; set; }


         [XmlElement("packageTrackInfo")]
public  		string
  packageTrackInfo { get; set; }


         [XmlElement("note")]
public  		string
  note { get; set; }


         [XmlElement("skuNum")]
public  		string
  skuNum { get; set; }


         [XmlElement("payTotalBuy")]
public  		string
  payTotalBuy { get; set; }


         [XmlElement("prdTotalBuy")]
public  		string
  prdTotalBuy { get; set; }


         [XmlElement("shipCostBuy")]
public  		string
  shipCostBuy { get; set; }


         [XmlElement("shipDisBuy")]
public  		string
  shipDisBuy { get; set; }


         [XmlElement("couponDisBuy")]
public  		string
  couponDisBuy { get; set; }


         [XmlElement("promDisBuy")]
public  		string
  promDisBuy { get; set; }


         [XmlElement("isDiscount")]
public  		string
  isDiscount { get; set; }


         [XmlElement("discountBuy")]
public  		string
  discountBuy { get; set; }


         [XmlElement("bookTime")]
public  		DateTime
  bookTime { get; set; }


         [XmlElement("payTime")]
public  		DateTime
  payTime { get; set; }


         [XmlElement("completeTime")]
public  		DateTime
  completeTime { get; set; }


         [XmlElement("shipTime")]
public  		DateTime
  shipTime { get; set; }


         [XmlElement("userIP")]
public  		string
  userIP { get; set; }


         [XmlElement("payType")]
public  		string
  payType { get; set; }


         [XmlElement("email")]
public  		string
  email { get; set; }


         [XmlElement("firstName")]
public  		string
  firstName { get; set; }


         [XmlElement("lastName")]
public  		string
  lastName { get; set; }


         [XmlElement("countryId")]
public  		string
  countryId { get; set; }


         [XmlElement("country")]
public  		string
  country { get; set; }


         [XmlElement("stateId")]
public  		string
  stateId { get; set; }


         [XmlElement("state")]
public  		string
  state { get; set; }


         [XmlElement("city")]
public  		string
  city { get; set; }


         [XmlElement("shipAddress1")]
public  		string
  shipAddress1 { get; set; }


         [XmlElement("shipAddress2")]
public  		string
  shipAddress2 { get; set; }


         [XmlElement("phone")]
public  		string
  phone { get; set; }


         [XmlElement("postCode")]
public  		string
  postCode { get; set; }


         [XmlElement("zip")]
public  		string
  zip { get; set; }


         [XmlElement("skus")]
public  		List<string>
  skus { get; set; }


         [XmlElement("messegeCode")]
public  		string
  messegeCode { get; set; }


         [XmlElement("message")]
public  		string
  message { get; set; }


         [XmlElement("isSuccess")]
public  		string
  isSuccess { get; set; }


}
}
