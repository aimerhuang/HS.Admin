using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ImSelfOrderInfoDTO : JdObject{


         [XmlElement("orderNo")]
public  		string
  orderNo { get; set; }


         [XmlElement("orderId")]
public  		long
  orderId { get; set; }


         [XmlElement("orderType")]
public  		int
  orderType { get; set; }


         [XmlElement("subCompanyId")]
public  		string
  subCompanyId { get; set; }


         [XmlElement("subCompanyName")]
public  		string
  subCompanyName { get; set; }


         [XmlElement("operationCenterId")]
public  		string
  operationCenterId { get; set; }


         [XmlElement("operationCenterName")]
public  		string
  operationCenterName { get; set; }


         [XmlElement("outletsId")]
public  		string
  outletsId { get; set; }


         [XmlElement("outletsName")]
public  		string
  outletsName { get; set; }


         [XmlElement("signingDate")]
public  		DateTime
  signingDate { get; set; }


         [XmlElement("expectAtHomeDate")]
public  		DateTime
  expectAtHomeDate { get; set; }


         [XmlElement("atHomeFinishDate")]
public  		DateTime
  atHomeFinishDate { get; set; }


         [XmlElement("serviceOrderFinishDate")]
public  		DateTime
  serviceOrderFinishDate { get; set; }


         [XmlElement("feedbackChannelDate")]
public  		DateTime
  feedbackChannelDate { get; set; }


         [XmlElement("productCode")]
public  		string
  productCode { get; set; }


         [XmlElement("productName")]
public  		string
  productName { get; set; }


         [XmlElement("productBrand")]
public  		int
  productBrand { get; set; }


         [XmlElement("productBrandName")]
public  		string
  productBrandName { get; set; }


         [XmlElement("productType")]
public  		int
  productType { get; set; }


         [XmlElement("productTypeName")]
public  		string
  productTypeName { get; set; }


         [XmlElement("jdWareId")]
public  		string
  jdWareId { get; set; }


         [XmlElement("factoryWareId")]
public  		string
  factoryWareId { get; set; }


         [XmlElement("factoryId")]
public  		int
  factoryId { get; set; }


         [XmlElement("factoryName")]
public  		string
  factoryName { get; set; }


         [XmlElement("freeInstall")]
public  		int
  freeInstall { get; set; }


         [XmlElement("freeInstallName")]
public  		string
  freeInstallName { get; set; }


         [XmlElement("insideId")]
public  		string
  insideId { get; set; }


         [XmlElement("outsideId")]
public  		string
  outsideId { get; set; }


         [XmlElement("engineerFirst")]
public  		string
  engineerFirst { get; set; }


         [XmlElement("engineerFirstName")]
public  		string
  engineerFirstName { get; set; }


         [XmlElement("engineerFirstPhone")]
public  		string
  engineerFirstPhone { get; set; }


         [XmlElement("engineerSecond")]
public  		string
  engineerSecond { get; set; }


         [XmlElement("engineerSecondName")]
public  		string
  engineerSecondName { get; set; }


         [XmlElement("engineerSecondPhone")]
public  		string
  engineerSecondPhone { get; set; }


         [XmlElement("scheduleCustomerName")]
public  		string
  scheduleCustomerName { get; set; }


         [XmlElement("scheduleCustomerPhone")]
public  		string
  scheduleCustomerPhone { get; set; }


         [XmlElement("scheduleServiceStreet")]
public  		string
  scheduleServiceStreet { get; set; }


         [XmlElement("serviceProvinceId")]
public  		string
  serviceProvinceId { get; set; }


         [XmlElement("serviceCityId")]
public  		string
  serviceCityId { get; set; }


         [XmlElement("serviceCountyId")]
public  		string
  serviceCountyId { get; set; }


         [XmlElement("serviceDistrictId")]
public  		string
  serviceDistrictId { get; set; }


         [XmlElement("serviceProvince")]
public  		string
  serviceProvince { get; set; }


         [XmlElement("serviceCity")]
public  		string
  serviceCity { get; set; }


         [XmlElement("serviceCounty")]
public  		string
  serviceCounty { get; set; }


         [XmlElement("serviceDistrict")]
public  		string
  serviceDistrict { get; set; }


         [XmlElement("createOrderDate")]
public  		DateTime
  createOrderDate { get; set; }


         [XmlElement("orderStatus")]
public  		int
  orderStatus { get; set; }


         [XmlElement("orderStatusName")]
public  		string
  orderStatusName { get; set; }


         [XmlElement("orderOrderStatus")]
public  		int
  orderOrderStatus { get; set; }


         [XmlElement("orderOrderStatusName")]
public  		string
  orderOrderStatusName { get; set; }


}
}
