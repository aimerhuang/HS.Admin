using System;
using System.Xml.Serialization;
using System.Collections.Generic;

using Extra.UpGrade.SDK.JingDong.Domain;
namespace Extra.UpGrade.SDK.JingDong.Domain
{





[Serializable]
public class StockChangeDetail : JdObject{


         [XmlElement("ownerNo")]
public  		string
  ownerNo { get; set; }


         [XmlElement("skuNo")]
public  		string
  skuNo { get; set; }


         [XmlElement("productLevel")]
public  		string
  productLevel { get; set; }


         [XmlElement("changeQty")]
public  		string
  changeQty { get; set; }


         [XmlElement("toOwnerNo")]
public  		string
  toOwnerNo { get; set; }


         [XmlElement("toSkuNo")]
public  		string
  toSkuNo { get; set; }


         [XmlElement("toProductLevel")]
public  		string
  toProductLevel { get; set; }


}
}
