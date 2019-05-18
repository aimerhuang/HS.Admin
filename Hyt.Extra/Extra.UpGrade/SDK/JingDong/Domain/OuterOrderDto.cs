using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class OuterOrderDto : JdObject{


         [XmlElement("orderNo")]
public  		string
  orderNo { get; set; }


         [XmlElement("orderStatus")]
public  		int
  orderStatus { get; set; }


         [XmlElement("warehouseNo")]
public  		string
  warehouseNo { get; set; }


         [XmlElement("tenantId")]
public  		string
  tenantId { get; set; }


         [XmlElement("invoiceQrcode")]
public  		string
  invoiceQrcode { get; set; }


         [XmlElement("carrierNo")]
public  		string
  carrierNo { get; set; }


         [XmlElement("updateTime")]
public  		DateTime
  updateTime { get; set; }


         [XmlElement("orderDs")]
public  		List<string>
  orderDs { get; set; }


         [XmlElement("orderStatusDtos")]
public  		List<string>
  orderStatusDtos { get; set; }


         [XmlElement("waybillNoList")]
public  		List<string>
  waybillNoList { get; set; }


}
}
