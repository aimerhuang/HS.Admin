using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class JosProductInfoDto : JdObject{


         [XmlElement("ware_id")]
public  		string
  wareId { get; set; }


         [XmlElement("name")]
public  		string
  name { get; set; }


         [XmlElement("cid1")]
public  		int
  cid1 { get; set; }


         [XmlElement("cid1_name")]
public  		string
  cid1Name { get; set; }


         [XmlElement("cid2")]
public  		int
  cid2 { get; set; }


         [XmlElement("cid2_name")]
public  		string
  cid2Name { get; set; }


         [XmlElement("model")]
public  		string
  model { get; set; }


         [XmlElement("original_place")]
public  		string
  originalPlace { get; set; }


         [XmlElement("length")]
public  		int
  length { get; set; }


         [XmlElement("width")]
public  		int
  width { get; set; }


         [XmlElement("height")]
public  		int
  height { get; set; }


         [XmlElement("weight")]
public  		string
  weight { get; set; }


         [XmlElement("saler_code")]
public  		string
  salerCode { get; set; }


         [XmlElement("saler_name")]
public  		string
  salerName { get; set; }


         [XmlElement("purchaser_code")]
public  		string
  purchaserCode { get; set; }


         [XmlElement("purchaser_name")]
public  		string
  purchaserName { get; set; }


         [XmlElement("market_price")]
public  		string
  marketPrice { get; set; }


         [XmlElement("member_price")]
public  		string
  memberPrice { get; set; }


         [XmlElement("brand_id")]
public  		int
  brandId { get; set; }


         [XmlElement("brand_name")]
public  		string
  brandName { get; set; }


         [XmlElement("zh_brand")]
public  		string
  zhBrand { get; set; }


         [XmlElement("en_brand")]
public  		string
  enBrand { get; set; }


         [XmlElement("warranty")]
public  		string
  warranty { get; set; }


         [XmlElement("shelf_life")]
public  		int
  shelfLife { get; set; }


         [XmlElement("pkg_info")]
public  		string
  pkgInfo { get; set; }


         [XmlElement("w_readme")]
public  		string
  wReadme { get; set; }


         [XmlElement("sku_unit")]
public  		string
  skuUnit { get; set; }


         [XmlElement("web_site")]
public  		string
  webSite { get; set; }


         [XmlElement("tel")]
public  		string
  tel { get; set; }


         [XmlElement("upc")]
public  		string
  upc { get; set; }


         [XmlElement("packing")]
public  		int
  packing { get; set; }


         [XmlElement("intro")]
public  		string
  intro { get; set; }


         [XmlElement("sale_state")]
public  		int
  saleState { get; set; }


         [XmlElement("pack_type")]
public  		int
  packType { get; set; }


         [XmlElement("modify_time")]
public  		DateTime
  modifyTime { get; set; }


         [XmlElement("props")]
public  		string
  props { get; set; }


         [XmlElement("input_pids")]
public  		string
  inputPids { get; set; }


         [XmlElement("input_str")]
public  		string
  inputStr { get; set; }


}
}
