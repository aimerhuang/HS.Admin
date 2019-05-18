using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class OrderDetailResult : JdObject{


         [XmlElement("eclpSoNo")]
public  		string
  eclpSoNo { get; set; }


         [XmlElement("isvUUID")]
public  		string
  isvUUID { get; set; }


         [XmlElement("shopNo")]
public  		string
  shopNo { get; set; }


         [XmlElement("departmentNo")]
public  		string
  departmentNo { get; set; }


         [XmlElement("warehouseNo")]
public  		string
  warehouseNo { get; set; }


         [XmlElement("shipperNo")]
public  		string
  shipperNo { get; set; }


         [XmlElement("shipperName")]
public  		string
  shipperName { get; set; }


         [XmlElement("salesPlatformOrderNo")]
public  		string
  salesPlatformOrderNo { get; set; }


         [XmlElement("salePlatformSource")]
public  		string
  salePlatformSource { get; set; }


         [XmlElement("salesPlatformCreateTime")]
public  		DateTime
  salesPlatformCreateTime { get; set; }


         [XmlElement("consigneeName")]
public  		string
  consigneeName { get; set; }


         [XmlElement("consigneeMobile")]
public  		string
  consigneeMobile { get; set; }


         [XmlElement("consigneePhone")]
public  		string
  consigneePhone { get; set; }


         [XmlElement("consigneeEmail")]
public  		string
  consigneeEmail { get; set; }


         [XmlElement("expectDate")]
public  		DateTime
  expectDate { get; set; }


         [XmlElement("addressProvince")]
public  		string
  addressProvince { get; set; }


         [XmlElement("addressCity")]
public  		string
  addressCity { get; set; }


         [XmlElement("addressCounty")]
public  		string
  addressCounty { get; set; }


         [XmlElement("addressTown")]
public  		string
  addressTown { get; set; }


         [XmlElement("consigneeAddress")]
public  		string
  consigneeAddress { get; set; }


         [XmlElement("consigneePostcode")]
public  		string
  consigneePostcode { get; set; }


         [XmlElement("receivable")]
public  		double
  receivable { get; set; }


         [XmlElement("consigneeRemark")]
public  		string
  consigneeRemark { get; set; }


         [XmlElement("orderMark")]
public  		string
  orderMark { get; set; }


         [XmlElement("afterSalesName")]
public  		string
  afterSalesName { get; set; }


         [XmlElement("afterSalesMobile")]
public  		string
  afterSalesMobile { get; set; }


         [XmlElement("afterSalesAddress")]
public  		string
  afterSalesAddress { get; set; }


         [XmlElement("pinAccount")]
public  		string
  pinAccount { get; set; }


         [XmlElement("splitFlag")]
public  		string
  splitFlag { get; set; }


         [XmlElement("currentStatus")]
public  		string
  currentStatus { get; set; }


         [XmlElement("wayBill")]
public  		string
  wayBill { get; set; }


         [XmlElement("orderDetailList")]
public  		List<string>
  orderDetailList { get; set; }


         [XmlElement("splitEclpSoNos")]
public  		string
  splitEclpSoNos { get; set; }


         [XmlElement("orderPackageList")]
public  		List<string>
  orderPackageList { get; set; }


         [XmlElement("orderStatusList")]
public  		List<string>
  orderStatusList { get; set; }


}
}
