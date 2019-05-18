using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class BookEntity : JdObject{


         [XmlElement("sku_id")]
public  		int
  skuId { get; set; }


         [XmlElement("book_info")]
public  		string
  bookInfo { get; set; }


}
}
