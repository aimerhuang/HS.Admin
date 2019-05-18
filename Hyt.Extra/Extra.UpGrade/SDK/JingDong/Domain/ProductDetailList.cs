using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class ProductDetailList : JdObject{


         [XmlElement("resultCode")]
public  		int
  resultCode { get; set; }


         [XmlElement("productInfo")]
public  		string
  productInfo { get; set; }


         [XmlElement("imagePathsList")]
public  		List<string>
  imagePathsList { get; set; }


}
}
