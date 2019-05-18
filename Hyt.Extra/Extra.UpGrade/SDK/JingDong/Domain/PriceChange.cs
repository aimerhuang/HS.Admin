using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class PriceChange : JdObject{


         [XmlElement("sku_id")]
public  		string
  skuId { get; set; }


         [XmlElement("price")]
public  		string
  price { get; set; }


         [XmlElement("market_price")]
public  		string
  marketPrice { get; set; }


}
}
