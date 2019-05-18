using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class Ware : JdObject{


         [XmlElement("wareId")]
public  		long
  wareId { get; set; }


         [XmlElement("title")]
public  		string
  title { get; set; }


         [XmlElement("categoryId")]
public  		long
  categoryId { get; set; }


         [XmlElement("brandId")]
public  		long
  brandId { get; set; }


         [XmlElement("templateId")]
public  		long
  templateId { get; set; }


         [XmlElement("transportId")]
public  		long
  transportId { get; set; }


         [XmlElement("wareStatus")]
public  		int
  wareStatus { get; set; }


         [XmlElement("outerId")]
public  		string
  outerId { get; set; }


         [XmlElement("itemNum")]
public  		string
  itemNum { get; set; }


         [XmlElement("barCode")]
public  		string
  barCode { get; set; }


         [XmlElement("wareLocation")]
public  		int
  wareLocation { get; set; }


         [XmlElement("modified")]
public  		DateTime
  modified { get; set; }


         [XmlElement("created")]
public  		DateTime
  created { get; set; }


         [XmlElement("offlineTime")]
public  		DateTime
  offlineTime { get; set; }


         [XmlElement("onlineTime")]
public  		DateTime
  onlineTime { get; set; }


         [XmlElement("colType")]
public  		int
  colType { get; set; }


         [XmlElement("delivery")]
public  		string
  delivery { get; set; }


         [XmlElement("adWords")]
public  		string
  adWords { get; set; }


         [XmlElement("wrap")]
public  		string
  wrap { get; set; }


         [XmlElement("packListing")]
public  		string
  packListing { get; set; }


         [XmlElement("weight")]
public  		string
  weight { get; set; }


         [XmlElement("width")]
public  		int
  width { get; set; }


         [XmlElement("height")]
public  		int
  height { get; set; }


         [XmlElement("length")]
public  		int
  length { get; set; }


         [XmlElement("props")]
public  		string
  props { get; set; }


         [XmlElement("features")]
public  		string
  features { get; set; }


         [XmlElement("images")]
public  		List<string>
  images { get; set; }


         [XmlElement("shopCategorys")]
public  		string
  shopCategorys { get; set; }


         [XmlElement("mobileDesc")]
public  		string
  mobileDesc { get; set; }


         [XmlElement("introduction")]
public  		string
  introduction { get; set; }


         [XmlElement("afterSales")]
public  		string
  afterSales { get; set; }


         [XmlElement("logo")]
public  		string
  logo { get; set; }


         [XmlElement("marketPrice")]
public  		string
  marketPrice { get; set; }


         [XmlElement("costPrice")]
public  		string
  costPrice { get; set; }


         [XmlElement("jdPrice")]
public  		string
  jdPrice { get; set; }


         [XmlElement("brandName")]
public  		string
  brandName { get; set; }


         [XmlElement("stockNum")]
public  		long
  stockNum { get; set; }


         [XmlElement("categorySecId")]
public  		long
  categorySecId { get; set; }


         [XmlElement("shopId")]
public  		long
  shopId { get; set; }


         [XmlElement("promiseId")]
public  		long
  promiseId { get; set; }


}
}
