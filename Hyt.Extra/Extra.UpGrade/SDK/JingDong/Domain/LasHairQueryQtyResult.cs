using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class LasHairQueryQtyResult : JdObject{


         [XmlElement("bookNo")]
public  		string
  bookNo { get; set; }


         [XmlElement("poNo")]
public  		string
  poNo { get; set; }


         [XmlElement("sku")]
public  		string
  sku { get; set; }


         [XmlElement("purchaseQty")]
public  		int
  purchaseQty { get; set; }


         [XmlElement("bookQty")]
public  		int
  bookQty { get; set; }


         [XmlElement("oldBookQty")]
public  		int
  oldBookQty { get; set; }


         [XmlElement("bookDate")]
public  		string
  bookDate { get; set; }


         [XmlElement("bookTimePeriod")]
public  		string
  bookTimePeriod { get; set; }


}
}
