using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class OrderPrintDataWare : JdObject{


         [XmlElement("ware_id")]
public  		string
  wareId { get; set; }


         [XmlElement("ware_name")]
public  		string
  wareName { get; set; }


         [XmlElement("num")]
public  		string
  num { get; set; }


         [XmlElement("jd_price")]
public  		string
  jdPrice { get; set; }


         [XmlElement("price")]
public  		string
  price { get; set; }


         [XmlElement("produce_no")]
public  		string
  produceNo { get; set; }


}
}
