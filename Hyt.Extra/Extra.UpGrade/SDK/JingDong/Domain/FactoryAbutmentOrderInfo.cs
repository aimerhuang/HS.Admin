using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class FactoryAbutmentOrderInfo : JdObject{


         [XmlElement("orderno")]
public  		string
  orderno { get; set; }


         [XmlElement("orderType")]
public  		int
  orderType { get; set; }


         [XmlElement("factoryAbutmentServiceInfoList")]
public  		List<string>
  factoryAbutmentServiceInfoList { get; set; }


         [XmlElement("orderServiceRemark")]
public  		string
  orderServiceRemark { get; set; }


         [XmlElement("authorizedSequence")]
public  		string
  authorizedSequence { get; set; }


         [XmlElement("customerName")]
public  		string
  customerName { get; set; }


         [XmlElement("customerTel")]
public  		string
  customerTel { get; set; }


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


         [XmlElement("serviceStreet")]
public  		string
  serviceStreet { get; set; }


         [XmlElement("jdWareId")]
public  		string
  jdWareId { get; set; }


         [XmlElement("factoryWareId")]
public  		string
  factoryWareId { get; set; }


         [XmlElement("productName")]
public  		string
  productName { get; set; }


         [XmlElement("orderId")]
public  		string
  orderId { get; set; }


         [XmlElement("serviceOrderId")]
public  		string
  serviceOrderId { get; set; }


         [XmlElement("createOrderTime")]
public  		DateTime
  createOrderTime { get; set; }


         [XmlElement("ImageUploadPath")]
public  		DateTime
  ImageUploadPath { get; set; }


         [XmlElement("ImageDownloadPath")]
public  		DateTime
  ImageDownloadPath { get; set; }


         [XmlElement("codDate")]
public  		DateTime
  codDate { get; set; }


         [XmlElement("daJiaDianInstallDate")]
public  		DateTime
  daJiaDianInstallDate { get; set; }


         [XmlElement("serviceDate")]
public  		DateTime
  serviceDate { get; set; }


         [XmlElement("expectAtHomePeriod")]
public  		string
  expectAtHomePeriod { get; set; }


}
}
