using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class StockBillSamDetail : JdObject{


         [XmlElement("item_id")]
public  		long
  itemId { get; set; }


         [XmlElement("sku_id")]
public  		long
  skuId { get; set; }


         [XmlElement("price")]
public  		double
  price { get; set; }


         [XmlElement("apply_num")]
public  		long
  applyNum { get; set; }


         [XmlElement("apply_money")]
public  		double
  applyMoney { get; set; }


         [XmlElement("real_num")]
public  		long
  realNum { get; set; }


         [XmlElement("real_money")]
public  		double
  realMoney { get; set; }


         [XmlElement("remark")]
public  		string
  remark { get; set; }


}
}
