using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class WareApiVO : JdObject{


         [XmlElement("wareId")]
public  		long
  wareId { get; set; }


         [XmlElement("categoryId")]
public  		int
  categoryId { get; set; }


         [XmlElement("wareStatus")]
public  		int
  wareStatus { get; set; }


         [XmlElement("title")]
public  		string
  title { get; set; }


         [XmlElement("itemNum")]
public  		string
  itemNum { get; set; }


         [XmlElement("transportId")]
public  		long
  transportId { get; set; }


         [XmlElement("onlineTime")]
public  		DateTime
  onlineTime { get; set; }


         [XmlElement("offlineTime")]
public  		DateTime
  offlineTime { get; set; }


         [XmlElement("attributes")]
public  		string
  attributes { get; set; }


         [XmlElement("minSupplyPrice")]
public  		string
  minSupplyPrice { get; set; }


         [XmlElement("maxSupplyPrice")]
public  		string
  maxSupplyPrice { get; set; }


         [XmlElement("stock")]
public  		int
  stock { get; set; }


         [XmlElement("imgUri")]
public  		string
  imgUri { get; set; }


         [XmlElement("hsCode")]
public  		string
  hsCode { get; set; }


         [XmlElement("recommendTpid")]
public  		int
  recommendTpid { get; set; }


         [XmlElement("customTpid")]
public  		int
  customTpid { get; set; }


         [XmlElement("brandId")]
public  		int
  brandId { get; set; }


         [XmlElement("deliveryDays")]
public  		int
  deliveryDays { get; set; }


         [XmlElement("keywords")]
public  		string
  keywords { get; set; }


         [XmlElement("description")]
public  		string
  description { get; set; }


         [XmlElement("cubage")]
public  		string
  cubage { get; set; }


         [XmlElement("packInfo")]
public  		string
  packInfo { get; set; }


         [XmlElement("netWeight")]
public  		string
  netWeight { get; set; }


         [XmlElement("weight")]
public  		string
  weight { get; set; }


         [XmlElement("packLong")]
public  		string
  packLong { get; set; }


         [XmlElement("packWide")]
public  		string
  packWide { get; set; }


         [XmlElement("packHeight")]
public  		string
  packHeight { get; set; }


         [XmlElement("wareSkus")]
public  		string
  wareSkus { get; set; }


         [XmlElement("messegeCode")]
public  		string
  messegeCode { get; set; }


         [XmlElement("message")]
public  		string
  message { get; set; }


         [XmlElement("success")]
public  		string
  success { get; set; }


}
}
