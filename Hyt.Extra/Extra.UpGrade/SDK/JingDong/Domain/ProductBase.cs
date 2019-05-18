using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ProductBase : JdObject{


         [XmlElement("skuId")]
public  		long
  skuId { get; set; }


         [XmlElement("name")]
public  		string
  name { get; set; }


         [XmlElement("isDelete")]
public  		string
  isDelete { get; set; }


         [XmlElement("state")]
public  		string
  state { get; set; }


         [XmlElement("barCode")]
public  		string
  barCode { get; set; }


         [XmlElement("erpPid")]
public  		string
  erpPid { get; set; }


         [XmlElement("color")]
public  		string
  color { get; set; }


         [XmlElement("colorSequence")]
public  		string
  colorSequence { get; set; }


         [XmlElement("size")]
public  		string
  size { get; set; }


         [XmlElement("sizeSequence")]
public  		string
  sizeSequence { get; set; }


         [XmlElement("upc")]
public  		string
  upc { get; set; }


         [XmlElement("skuMark")]
public  		string
  skuMark { get; set; }


         [XmlElement("saleDate")]
public  		string
  saleDate { get; set; }


         [XmlElement("cid2")]
public  		string
  cid2 { get; set; }


         [XmlElement("valueWeight")]
public  		string
  valueWeight { get; set; }


         [XmlElement("weight")]
public  		string
  weight { get; set; }


         [XmlElement("productArea")]
public  		string
  productArea { get; set; }


         [XmlElement("wserve")]
public  		string
  wserve { get; set; }


         [XmlElement("allnum")]
public  		string
  allnum { get; set; }


         [XmlElement("maxPurchQty")]
public  		string
  maxPurchQty { get; set; }


         [XmlElement("brandId")]
public  		string
  brandId { get; set; }


         [XmlElement("valuePayFirst")]
public  		string
  valuePayFirst { get; set; }


         [XmlElement("length")]
public  		string
  length { get; set; }


         [XmlElement("width")]
public  		string
  width { get; set; }


         [XmlElement("height")]
public  		string
  height { get; set; }


         [XmlElement("venderType")]
public  		string
  venderType { get; set; }


         [XmlElement("pname")]
public  		string
  pname { get; set; }


         [XmlElement("issn")]
public  		string
  issn { get; set; }


         [XmlElement("safeDays")]
public  		string
  safeDays { get; set; }


         [XmlElement("saleUnit")]
public  		string
  saleUnit { get; set; }


         [XmlElement("packSpecification")]
public  		string
  packSpecification { get; set; }


         [XmlElement("category")]
public  		string
  category { get; set; }


         [XmlElement("shopCategorys")]
public  		string
  shopCategorys { get; set; }


         [XmlElement("phone")]
public  		string
  phone { get; set; }


         [XmlElement("site")]
public  		string
  site { get; set; }


         [XmlElement("ebrand")]
public  		string
  ebrand { get; set; }


         [XmlElement("cbrand")]
public  		string
  cbrand { get; set; }


         [XmlElement("model")]
public  		string
  model { get; set; }


         [XmlElement("imagePath")]
public  		string
  imagePath { get; set; }


         [XmlElement("shopName")]
public  		string
  shopName { get; set; }


         [XmlElement("url")]
public  		string
  url { get; set; }


}
}
