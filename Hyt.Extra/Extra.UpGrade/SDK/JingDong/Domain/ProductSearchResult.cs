using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ProductSearchResult : JdObject{


         [XmlElement("code")]
public  		string
  code { get; set; }


         [XmlElement("total")]
public  		string
  total { get; set; }


         [XmlElement("scrollId")]
public  		string
  scrollId { get; set; }


         [XmlElement("skuList")]
public  		List<string>
  skuList { get; set; }


}
}
