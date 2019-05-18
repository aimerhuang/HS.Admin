using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class StockQueryResultDto : JdObject{


         [XmlElement("stockNum")]
public  		int
  stockNum { get; set; }


         [XmlElement("orderBookingNum")]
public  		int
  orderBookingNum { get; set; }


         [XmlElement("wname")]
public  		string
  wname { get; set; }


         [XmlElement("sku")]
public  		long
  sku { get; set; }


}
}
