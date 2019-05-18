using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class JosStockSalesDetail : JdObject{


         [XmlElement("product_code")]
public  		string
  productCode { get; set; }


         [XmlElement("product_name")]
public  		string
  productName { get; set; }


         [XmlElement("stock_count")]
public  		long
  stockCount { get; set; }


         [XmlElement("sales")]
public  		long
  sales { get; set; }


}
}
