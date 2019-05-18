using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class WaybillDTO : JdObject{


         [XmlElement("waybillType")]
public  		int
  waybillType { get; set; }


         [XmlElement("waybillCodes")]
public  		List<string>
  waybillCodes { get; set; }


         [XmlElement("waybillCount")]
public  		int
  waybillCount { get; set; }


         [XmlElement("providerId")]
public  		int
  providerId { get; set; }


         [XmlElement("providerCode")]
public  		string
  providerCode { get; set; }


         [XmlElement("branchCode")]
public  		string
  branchCode { get; set; }


         [XmlElement("platformOrderNo")]
public  		string
  platformOrderNo { get; set; }


         [XmlElement("vendorCode")]
public  		string
  vendorCode { get; set; }


         [XmlElement("vendorName")]
public  		string
  vendorName { get; set; }


         [XmlElement("vendorOrderCode")]
public  		string
  vendorOrderCode { get; set; }


         [XmlElement("salePlatform")]
public  		string
  salePlatform { get; set; }


         [XmlElement("fromAddress")]
public  		string
  fromAddress { get; set; }


         [XmlElement("toAddress")]
public  		string
  toAddress { get; set; }


         [XmlElement("weight")]
public  		string
  weight { get; set; }


         [XmlElement("volume")]
public  		string
  volume { get; set; }


         [XmlElement("goodsName")]
public  		string
  goodsName { get; set; }


         [XmlElement("promiseTimeType")]
public  		int
  promiseTimeType { get; set; }


         [XmlElement("promiseCompleteTime")]
public  		DateTime
  promiseCompleteTime { get; set; }


         [XmlElement("goodsMoney")]
public  		string
  goodsMoney { get; set; }


         [XmlElement("payType")]
public  		int
  payType { get; set; }


         [XmlElement("shouldPayMoney")]
public  		string
  shouldPayMoney { get; set; }


         [XmlElement("needGuarantee")]
public  		bool
  needGuarantee { get; set; }


         [XmlElement("guaranteeMoney")]
public  		string
  guaranteeMoney { get; set; }


         [XmlElement("state")]
public  		int
  state { get; set; }


         [XmlElement("receiveTimeType")]
public  		int
  receiveTimeType { get; set; }


         [XmlElement("warehouseCode")]
public  		string
  warehouseCode { get; set; }


         [XmlElement("secondSectionCode")]
public  		string
  secondSectionCode { get; set; }


         [XmlElement("thirdSectionCode")]
public  		string
  thirdSectionCode { get; set; }


         [XmlElement("remark")]
public  		string
  remark { get; set; }


         [XmlElement("extendField1")]
public  		string
  extendField1 { get; set; }


         [XmlElement("extendField2")]
public  		string
  extendField2 { get; set; }


         [XmlElement("extendField3")]
public  		string
  extendField3 { get; set; }


         [XmlElement("extendField4")]
public  		string
  extendField4 { get; set; }


         [XmlElement("extendField5")]
public  		string
  extendField5 { get; set; }


         [XmlElement("pin")]
public  		string
  pin { get; set; }


         [XmlElement("appKey")]
public  		string
  appKey { get; set; }


         [XmlElement("createTime")]
public  		DateTime
  createTime { get; set; }


         [XmlElement("expressPayMethod")]
public  		string
  expressPayMethod { get; set; }


         [XmlElement("expressType")]
public  		string
  expressType { get; set; }


         [XmlElement("settlementCode")]
public  		string
  settlementCode { get; set; }


         [XmlElement("existWaybillCode")]
public  		bool
  existWaybillCode { get; set; }


         [XmlElement("signReturn")]
public  		int
  signReturn { get; set; }


}
}
