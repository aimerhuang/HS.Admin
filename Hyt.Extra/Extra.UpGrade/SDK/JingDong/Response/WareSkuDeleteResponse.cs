using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Response
{





public class WareSkuDeleteResponse : JdResponse{


         [XmlElement("code")]
public  		string
  code { get; set; }


         [XmlElement("sku_id")]
public  		string
  skuId { get; set; }


         [XmlElement("modified")]
public  		string
  modified { get; set; }


}
}
